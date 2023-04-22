// See https://aka.ms/new-console-template for more information
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using System.Net.NetworkInformation;

Console.WriteLine("Hello, World!");

var services = new ServiceCollection();
services.AddMediator(opt =>
{
    opt.Namespace = null;
    opt.ServiceLifetime = ServiceLifetime.Transient;
});


var serviceProvider = services.BuildServiceProvider();

public sealed record Ping(Guid Id) : IRequest<Pong>;
public sealed record Pong(Guid Id);

public sealed class PingHandler : IRequestHandler<Ping, Pong>
{
    public ValueTask<Pong> Handle(Ping request, CancellationToken cancellationToken)
    {
        Console.WriteLine("4) Returning pong!");
        return new ValueTask<Pong>(new Pong(request.Id));
    }
}