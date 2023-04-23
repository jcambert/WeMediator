using We.Results;

namespace WeMediator;

public record Response
{
    public static implicit operator Result<Response>(Response response)
        => Result.Create(response);

    public static implicit operator Task<Result<Response>>(Response response)
        => Task.FromResult(Result.Create(response));
}
