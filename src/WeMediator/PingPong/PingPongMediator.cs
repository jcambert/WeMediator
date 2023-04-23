using Microsoft.Extensions.DependencyInjection;
using We.Results;

namespace WeMediator;
public partial interface IMediator
{
    //Task<Result<Pong>> Send(Ping request, CancellationToken cancellationToken = default);
}
public partial class Mediator
{
   
    public async Task<Result<Pong>> Send(Ping request, CancellationToken cancellationToken = default)
    {
        
    
            return await InternalHandling<Ping,Pong>(request,cancellationToken);
           /* var pipes = GetPipes<Ping, Pong>();

            Result resultat = Result.ValidWithFailure();
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
                        return await handlerCopy(r, c);

                    return result;
                };

            }
            var root = handler;
            var resultTask = root(request, cancellationToken);
            var result = await resultTask;

            if (result.Value is not null && resultat.Errors.Count > 0)
                return Result.ValidWithFailure<Pong>(result.Value, resultat.Errors.ToArray());
            if (result.Value is null && resultat.Errors.Count > 0)
                return Result.Failure<Pong>(resultat.Errors.ToArray());
            return result;
        }*/
        /*var handler = GetHandle<Ping, Pong>();
        return await handler(request, cancellationToken);*/

    }
    public Task<Result<Pong1>> Send(Ping1 request, CancellationToken cancellationToken = default)
    {
        var handler = ServiceProvider.GetService<IRequestHandler<Ping1, Pong1>>();
        return handler.Handle(request, cancellationToken);
    }
}
