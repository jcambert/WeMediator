using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using We.Results;
using WeMediator.Some;

namespace WeMediator;
public partial interface IMediator : ISender
{

}

/*
public partial interface IMediator<TRequest, TResponse> : ISender<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Response
{

}*/

public abstract class BaseMediator:IMediator
{
    protected readonly IServiceProvider ServiceProvider;
    public BaseMediator(IServiceProvider serviceProvider)
    {
        this.ServiceProvider = serviceProvider;
    }
    protected RequestHandlerDelegate<TRequest, TResponse> GetHandle<TRequest,TResponse>()
        where TRequest : IRequest<TResponse>
        where TResponse : Response
    {
        var type= typeof(TRequest);
        return (RequestHandlerDelegate<TRequest, TResponse>)(ServiceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>()).Handle;
        /*var h=(ServiceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>()).Handle;
        return default;*/
    }

    public async Task<Result<TResponse>> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        
        where TResponse : Response
        => await InternalHandling<IRequest<TResponse>, TResponse>(request, cancellationToken);

    protected IEnumerable<IPipelineHandler<TRequest, TResponse>> GetPipes<TRequest, TResponse>()
        where TRequest : IRequest<TResponse>
        where TResponse : Response
        => ServiceProvider.GetServices<IPipelineHandler<TRequest, TResponse>>();

    protected abstract Task<Result<TResponse>> InternalHandling<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
       where TRequest : IRequest<TResponse>
        where TResponse: Response;

}
/*
public class Mediator<TRequest, TResponse> : BaseMediator, IMediator<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Response
{
    public Mediator(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }


    public Task<Result<TResponse>> Send(TRequest request, CancellationToken CancellationToken = default)
    {
        var handler = GetHandle<TRequest, TResponse>();
        return handler(request, CancellationToken);
    }


}*/
public partial class Mediator : BaseMediator, IMediator
{

    public Mediator(IServiceProvider serviceProvider) : base(serviceProvider)
    {

    }



    /*{
        Result<TResponse> res = request switch
        {
            Ping r => Unsafe.As<Result<TResponse>>(await Send(r, cancellationToken)),
            Ping1 r => Unsafe.As<Result<TResponse>>(await Send(r, cancellationToken)),
            _ => Unsafe.As<Result<TResponse>>(await InternalHandling<IRequest<TResponse>, TResponse>(request, cancellationToken)),
        };
        return res;
    }*/




    protected override async Task<Result<TResponse>> InternalHandling<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
    {

        var rootHandler = GetHandle<TRequest, TResponse>();

        var pipes = GetPipes<TRequest, TResponse>();

        Result resultat = Result.ValidWithFailure();
        foreach (var pipe in pipes.Reverse())
        {

            var handlerCopy = rootHandler;
            var pipelineCopy = pipe;
            rootHandler = async (TRequest r, CancellationToken c) =>
            {
                Result<TResponse> result;
                try
                {
                    result = await pipe.Handle(r, handlerCopy, c);

                }
                catch (Exception ex)
                {
                    result = Result.Failure<TResponse>(ex);

                }

                if (result.HasError)
                    resultat.AddErrors(result.Errors);

                if (result && result.IsValidFailure)
                    return await handlerCopy(r, c);

                return result;
            };

        }
        var root = rootHandler;
        var resultTask = root(request, cancellationToken);
        var result = await resultTask;

        if (result.Value is not null && resultat.Errors.Count > 0)
            return Result.ValidWithFailure<TResponse>(result.Value, resultat.Errors.ToArray());
        if (result.Value is null && resultat.Errors.Count > 0)
            return Result.Failure<TResponse>(resultat.Errors.ToArray());
        return result;
    }

}

public delegate Task<Result<TResponse>> RequestHandlerDelegate<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
     where TRequest : IRequest<TResponse>;