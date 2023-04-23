using We.Results;

namespace WeMediator;

public interface ISender
{
    Task<Result<TResponse>> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
         where TResponse : Response;
    
    //  Task<Result<Pong>> Send(Ping request,CancellationToken cancellationToken=default);
}
/*
public interface ISender<TRequest,TResponse>
    where TRequest:IRequest<TResponse>
    where TResponse : Response
{
    Task<Result<TResponse>> Send(TRequest request,CancellationToken CancellationToken=default) ;
}*/
