using WeMediator;

namespace Microsoft.Extensions.DependencyInjection;
using SD = ServiceDescriptor;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {

        services.AddTransient<IMediator, Mediator>();
        services.AddTransient<IRequestHandler<Ping, Pong>, PingHandler>();
        services.AddTransient<IRequestHandler<Ping1, Pong1>, Ping1Handler>();
        services.AddTransient<IPipelineHandler<Ping, Pong>, PingValidator1>();
        services.AddTransient<IPipelineHandler<Ping, Pong>, PingValidator2>();

        return services;
    }
}

