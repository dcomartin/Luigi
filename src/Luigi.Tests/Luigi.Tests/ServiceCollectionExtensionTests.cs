using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Luigi.Tests
{
    public class ServiceCollectionExtensionTests
    {
        private readonly ServiceProvider _serviceProvider;

        public ServiceCollectionExtensionTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLuigi(typeof(HelloWorldPipeline).Assembly);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }
        
        [Fact]
        public void Should_resolve_IDispatcher()
        {
            var dispatcher = _serviceProvider.GetService<IDispatcher>();
            
            dispatcher.ShouldNotBeNull();
            dispatcher.ShouldBeOfType<Dispatcher>();
        }

        [Fact]
        public void Should_resolve_Pipeline()
        {
            var pipeline = _serviceProvider.GetService<IPipeline<HelloWorldRequest, string>>();
            
            pipeline.ShouldNotBeNull();
            pipeline.ShouldBeOfType<HelloWorldPipeline>();
        }

        [Fact]
        public void Should_resolve_Pipe()
        {
            var pipeline = _serviceProvider.GetService<HelloWorldPipe>();
            
            pipeline.ShouldNotBeNull();
        }
        
        [Fact]
        public void Should_resolve_Pipeline_with_PipeContext()
        {
            var pipeline = _serviceProvider.GetService<IPipeline<HelloWorldWithContextRequest, string, HelloWorldContext>>();
            
            pipeline.ShouldNotBeNull();
            pipeline.ShouldBeOfType<HelloWorldWithContextPipeline>();
        }
        
        [Fact]
        public void Should_resolve_Pipe_with_PipeContext()
        {
            var pipeline = _serviceProvider.GetService<HelloWorldWithContextPipe>();
            
            pipeline.ShouldNotBeNull();
        }
    }
}