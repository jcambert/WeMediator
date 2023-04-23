namespace WeMediator;

public sealed record Ping(Guid Id, bool UseValidator = true) : IRequest<Pong>;

public sealed record Pong(Guid Id, string Message = "") : Response;

public sealed record Ping1(Guid Id) : IRequest<Pong1>;

public sealed record Pong1(Guid Id, string Message = "") : Response;
