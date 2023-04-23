using We.Results;

namespace WeMediator;

public sealed class PingValidator1 : IPipelineHandler<Ping, Pong>
{
    //public bool ContinueOnFailure => true;

    public Task<Result<Pong>> Handle(Ping request, RequestHandlerDelegate<Ping, Pong> next, CancellationToken cancellationToken)
    {
        Console.WriteLine("1- Running ping validator 1");
        if (request is null || request.Id == default)
            //throw new ArgumentException("Invalid input");
            return Result.ValidWithFailure<Pong>(new ArgumentException("Invalid input 1->But continue"));
        else
            Console.WriteLine(@"2- Valid input!");
        return next(request, cancellationToken);
    }
}
