using Microsoft.Extensions.DependencyInjection.Extensions;
using WeMediator;

namespace Microsoft.Extensions.DependencyInjection;
using SD = ServiceDescriptor;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        services.Add(typeof(IMediator), typeof(Mediator), lifetime);
        services.Add(typeof(IMediator), sp => sp.GetRequiredService<Mediator>(), lifetime, true);
        services.Add(typeof(ISender), sp => sp.GetRequiredService<Mediator>(), lifetime, true);
        //services.AddMediatorService(typeof(IPublisher), sp => sp.GetRequiredService<Mediator>(), ServiceLifetime.Transient));

        
        services.Add(typeof(IRequestHandler<Ping, Pong>), typeof(PingHandler), lifetime);
        services.Add(typeof(IRequestHandler<Ping1, Pong1>), typeof(Ping1Handler), lifetime);

        /*


        services.AddTransient<IPipelineHandler<Ping, Pong>, PingValidator1>();
        services.AddTransient<IPipelineHandler<Ping, Pong>, PingValidator2>();

        */

        return services;
    }

    public static IServiceCollection Add(this IServiceCollection services, Type serviceType, Type implementationType, ServiceLifetime lifetime = ServiceLifetime.Transient, bool tryAdd = false)
    {
        Action<SD> fn = tryAdd ? services.TryAdd : services.Add;
        fn(new SD(serviceType, implementationType, lifetime));
        return services;

    }

    public static IServiceCollection Add(this IServiceCollection services, Type serviceType, Func<IServiceProvider, object> factory, ServiceLifetime lifetime = ServiceLifetime.Transient, bool tryAdd = false)
    {
        Action<SD> fn = tryAdd ? services.TryAdd : services.Add;
        fn(new SD(serviceType, factory, lifetime));
        return services;

    }

    public static IServiceCollection Add<TService,TImplementation>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient, bool tryAdd = false)
    {
        Action<SD> fn = tryAdd ? services.TryAdd : services.Add;
        fn(new SD(typeof(TService), typeof(TImplementation), lifetime));
        return services;
    }

    public static IServiceCollection AddRequestHandler<TRequest,TResponse, THandlerImplementation>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Transient, bool tryAdd = false)
        where THandlerImplementation : IRequestHandler<TRequest,TResponse>
        where TRequest:IRequest<TResponse>
    {
        //services.AddMediatorService(typeof(IRequestHandler<Ping, Pong>), typeof(PingHandler), lifetime);
        Action<SD> fn = tryAdd ? services.TryAdd : services.Add;
        //fn(new SD(typeof(IRequestHandler<TRequest,TResponse>), typeof(THandlerImplementation), lifetime));
        fn(new SD(typeof(TRequest), typeof(THandlerImplementation), lifetime));
        return services;
    }
        
}

