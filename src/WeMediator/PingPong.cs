using We.Results;

namespace WeMediator;

public sealed record Ping(Guid Id) : IRequest<Pong>;

public sealed record Pong(Guid Id,string Message=""):Response;


public sealed class PingHandler : IRequestHandler<Ping, Pong>
{
    public  Task<Result<Pong>> Handle(Ping request, CancellationToken cancellationToken)
    {
        var res = request.Id == Guid.Empty ? Guid.NewGuid() : request.Id;
        return Result.Create( new Pong(res, $"From {nameof(PingHandler)}"));
    }
}

public sealed record Ping1(Guid Id) : IRequest<Pong1>;

public sealed record Pong1(Guid Id, string Message = "") : Response;


public sealed class Ping1Handler : IRequestHandler<Ping1, Pong1>
{
    public Task<Result<Pong1>> Handle(Ping1 request, CancellationToken cancellationToken)
    {
        var res= request.Id == Guid.Empty?Guid.NewGuid():request.Id;
        return Result.Create(new Pong1(res,$"From {nameof(Ping1Handler)}"));
    }
}

public sealed class PingValidator1: IPipelineHandler<Ping, Pong>
{
    public bool ContinueOnFailure => true;

    public Task<Result<Pong>> Handle(Ping request, RequestHandlerDelegate<Ping,Pong> next, CancellationToken cancellationToken)
    {
        Console.WriteLine("1- Running ping validator 1");
        if (request is null || request.Id == default)
            //throw new ArgumentException("Invalid input");
            return Result.ValidWithFailure<Pong>(new ArgumentException("Invalid input 1"));
        else
            Console.WriteLine(@"2- Valid input!");
        return next(request, cancellationToken);
    }
}


public sealed class PingValidator2 : IPipelineHandler<Ping, Pong>
{
    public bool ContinueOnFailure => true;

    public Task<Result<Pong>> Handle(Ping request, RequestHandlerDelegate<Ping, Pong> next, CancellationToken cancellationToken)
    {
        Console.WriteLine("1- Running ping validator 2");
        if (request is null || request.Id == default)
        {
            request=new Ping(Guid.NewGuid());
            return Result.ValidWithFailure<Pong>(new ArgumentException("Invalid input 2\n Generate a new Guid"));
            //throw new ArgumentException("Invalid input 2\n Generate a new Guid");
        }
            
        else
            Console.WriteLine(@"2- Valid input!");
        return next(request, cancellationToken);
    }
}
