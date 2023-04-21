using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using We.Results;

namespace WeMediator;

public interface IRequest
{

}
public interface IRequest<out TResponse>
{

}

public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<Result<TResponse>> Handle(TRequest request, CancellationToken cancellationToken);
}



public interface ISender
{
    Task<Result<TResponse>> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
         where TResponse : Response;

    //  Task<Result<Pong>> Send(Ping request,CancellationToken cancellationToken=default);
}
public interface IMediator : ISender
{

}

public partial class Mediator : IMediator
{
    private readonly IServiceProvider _sp;
    public Mediator(IServiceProvider serviceProvider)
    {
        this._sp = serviceProvider;
    }



    public async Task<Result<Pong>> Send(Ping request, CancellationToken cancellationToken = default)
    {
        var handler = (RequestHandlerDelegate<Ping, Pong>)(_sp.GetRequiredService<IRequestHandler<Ping, Pong>>()).Handle;

        var pipes = _sp.GetServices<IPipelineHandler<Ping, Pong>>();

        Result resultat=Result.ValidWithFailure();
        foreach (var pipe in pipes.Reverse())
        {
            
            var handlerCopy = handler;
            var pipelineCopy = pipe;
            handler = async (Ping r, CancellationToken c) =>
            {
                Result<Pong> result;
                try
                {
                    result = await pipe.Handle(r, handlerCopy, c);
                   
                }
                catch (Exception ex)
                {
                    result = Result.Failure<Pong>(ex);

                }
                
                if (result.HasError)
                    resultat.AddErrors(result.Errors);
                
                if (result && result.IsValidFailure)
                    return await handlerCopy(r,c);

                return result;
            };

        }
        var root = handler;
        var resultTask= root(request, cancellationToken);
        var result = await resultTask;
        if(result.Value is not null &&  resultat.Errors.Count > 0)
            return Result.ValidWithFailure<Pong>(result.Value, resultat.Errors.ToArray()) ;
        if (result.Value is  null &&  resultat.Errors.Count > 0)
            return Result.Failure<Pong>(resultat.Errors.ToArray()) ;
        
        return result;
    }
    public Task<Result<Pong1>> Send(Ping1 request, CancellationToken cancellationToken = default)
    {
        var handler = _sp.GetService<IRequestHandler<Ping1, Pong1>>();
        return handler.Handle(request, cancellationToken);
    }
    public async Task<Result<TResponse>> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        where TResponse : Response
    {
        Result<TResponse> res = request switch
        {
            Ping r => Unsafe.As<Result<TResponse>>(await Send(r, cancellationToken)),
            Ping1 r => Unsafe.As<Result<TResponse>>(await Send(r, cancellationToken)),
            _ => throw new NotImplementedException(),
        };
        return res;
    }


}

public record Response
{
    public static implicit operator Result<Response>(Response response)
        => Result.Create(response);

    public static implicit operator Task<Result<Response>>(Response response)
        => Task.FromResult(Result.Create(response));
}

public interface IPipelineHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Pipeline handler. Perform any additional behavior and await the <paramref name="next"/> delegate as necessary
    /// </summary>
    /// <param name="request">Incoming request</param>
    /// <param name="next">Awaitable delegate for the next action in the pipeline. Eventually this delegate represents the handler.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Awaitable task returning the <typeparamref name="TResponse"/></returns>
    Task<Result<TResponse>> Handle(TRequest request, RequestHandlerDelegate<TRequest, TResponse> next, CancellationToken cancellationToken);

    bool ContinueOnFailure { get; }
}

public delegate Task<Result<TResponse>> RequestHandlerDelegate<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
     where TRequest : IRequest<TResponse>;