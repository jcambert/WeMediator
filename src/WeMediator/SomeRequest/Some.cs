using Microsoft.Extensions.DependencyInjection;
using We.Results;

namespace WeMediator.Some;


public sealed record SomeRequest(Guid Id) : IRequest<SomeResponse>;

public sealed record SomeResponse(Guid Id) : Response;
public sealed class SomeHandlerClass
    : IRequestHandler<SomeRequest, SomeResponse>
{
    private static readonly SomeResponse _response = new SomeResponse(Guid.NewGuid());
    private static readonly Task<Result<SomeResponse>> _tResponse = Task.FromResult(Result.Success(_response));


    Task<Result<SomeResponse>> IRequestHandler<SomeRequest, SomeResponse>.Handle(SomeRequest request, CancellationToken cancellationToken)
    => _tResponse;
}
/*
public partial interface ISomeMediator:ISender<SomeRequest, SomeResponse>
{
}

public partial class SomeMediator : Mediator<SomeRequest, SomeResponse>
{
    public SomeMediator(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}*/