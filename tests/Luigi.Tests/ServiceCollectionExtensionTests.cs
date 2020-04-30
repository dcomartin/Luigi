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
            serviceCollection.AddLuigi(typeof(HelloWorldQueryPipeline).Assembly);
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
        public void Should_resolve_QueryPipeline()
        {
            var pipeline = _serviceProvider.GetService<IQueryPipeline<HelloWorldQuery, string>>();
            
            pipeline.ShouldNotBeNull();
            pipeline.ShouldBeOfType<HelloWorldQueryPipeline>();
        }
        
        [Fact]
        public void Should_resolve_CommandPipeline()
        {
            var pipeline = _serviceProvider.GetService<ICommandPipeline<DoWorkCommand>>();
            
            pipeline.ShouldNotBeNull();
            pipeline.ShouldBeOfType<DoWorkCommandPipeline>();
        }

        [Fact]
        public void Should_resolve_QueryPipe()
        {
            var pipe = _serviceProvider.GetService<HelloWorldQueryPipe>();
            pipe.ShouldNotBeNull();
        }
        
        [Fact]
        public void Should_resolve_CommandPipe()
        {
            var pipe = _serviceProvider.GetService<DoWorkCommandPipe>();
            pipe.ShouldNotBeNull();
        }
        
        [Fact]
        public void Should_resolve_QueryPipeline_with_PipeContext()
        {
            var pipeline = _serviceProvider.GetService<IQueryPipeline<HelloWorldWithContextQuery, string, HelloWorldContext>>();
            
            pipeline.ShouldNotBeNull();
            pipeline.ShouldBeOfType<HelloWorldWithContextQueryPipeline>();
        }
        
        [Fact]
        public void Should_resolve_CommandPipeline_with_PipeContext()
        {
            var pipeline = _serviceProvider.GetService<ICommandPipeline<DoWorkWithContextCommand, DoWorkContext>>();
            
            pipeline.ShouldNotBeNull();
            pipeline.ShouldBeOfType<DoWorkWithContextCommandPipeline>();
        }
        
        [Fact]
        public void Should_resolve_QueryPipe_with_PipeContext()
        {
            var pipeline = _serviceProvider.GetService<HelloWorldWithContextQueryPipe>();
            
            pipeline.ShouldNotBeNull();
        }
    }
}