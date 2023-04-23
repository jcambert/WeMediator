using We.Results;

namespace WeMediator;

public sealed class PingValidator2 : IPipelineHandler<Ping, Pong>
{
    //public bool ContinueOnFailure => true;

    public Task<Result<Pong>> Handle(Ping request, RequestHandlerDelegate<Ping, Pong> next, CancellationToken cancellationToken)
    {
        Console.WriteLine("1- Running ping validator 2");
        if (request is null || request.Id == default)
        {
            request = new Ping(Guid.NewGuid());
            return Result.ValidWithFailure<Pong>(new ArgumentException("Invalid input 2->but continue\n Generate a new Guid"));
            //throw new ArgumentException("Invalid input 2\n Generate a new Guid");
        }

        else
            Console.WriteLine(@"2- Valid input!");
        return next(request, cancellationToken);
    }
}
