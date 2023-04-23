using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Microsoft.Extensions.DependencyInjection;
using We.Results;
using WeMediator.Benchmarks.Request;
using WeMediator.Some;
namespace WeMediator.Benchmarks.Request
{

    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
    [RankColumn]
    public class RequestBenchmarks
    {

        private IServiceProvider _serviceProvider;
        private IServiceScope _serviceScope;
        private IMediator _mediator;
        private static SomeRequest _some_request= new(Guid.NewGuid());
        private static Ping _ping_without_validator_request=new Ping(Guid.NewGuid(),false);
        private static Ping _ping_with_validator_request=new Ping(Guid.NewGuid(),true);
        [GlobalSetup]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddMediator(ServiceLifetime.Singleton);
            services.AddRequestHandler<SomeRequest,SomeResponse,SomeHandlerClass>( ServiceLifetime.Singleton);
            services.AddRequestHandler<Ping, Pong, PingHandler>(ServiceLifetime.Singleton);
            _serviceProvider = services.BuildServiceProvider();

            _serviceScope = _serviceProvider.CreateScope();

            _mediator = _serviceProvider.GetRequiredService<IMediator>();

            
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            if (_serviceScope is not null)
                _serviceScope.Dispose();
            else
                (_serviceProvider as IDisposable)?.Dispose();
        }

        [Benchmark]
        public Task<Result<SomeResponse>> SendRequest_SomeRequest_IMediator()
        {
            return _mediator.Send(_some_request, CancellationToken.None);
        }

        [Benchmark]
        public Task<Result<Pong>> SendRequest_PingRequest_WithoutValidator_IMediator()
        {
            return _mediator.Send(_ping_without_validator_request, CancellationToken.None);
        }

        [Benchmark]
        public Task<Result<Pong>> SendRequest_PingRequest_WithValidator_IMediator()
        {
            return _mediator.Send(_ping_with_validator_request, CancellationToken.None);
        }
    }
}

