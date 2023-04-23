using We.Results;

namespace WeMediator;

public sealed class Ping1Handler : IRequestHandler<Ping1, Pong1>
{
    public Task<Result<Pong1>> Handle(Ping1 request, CancellationToken cancellationToken)
    {
        var res = request.Id == Guid.Empty ? Guid.NewGuid() : request.Id;
        return Result.Create(new Pong1(res, $"From {nameof(Ping1Handler)}"));
    }
}
