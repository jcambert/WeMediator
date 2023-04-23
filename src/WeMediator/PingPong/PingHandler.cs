using We.Results;

namespace WeMediator;

public sealed class PingHandler : IRequestHandler<Ping, Pong>
{
    public Task<Result<Pong>> Handle(Ping request, CancellationToken cancellationToken)
    {
        var res = request.Id == Guid.Empty ? Guid.NewGuid() : request.Id;
        return Result.Create(new Pong(res, $"From {nameof(PingHandler)}"));
    }
}
