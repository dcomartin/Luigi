using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Luigi.Tests
{
    public class QueryPipelineTests
    {
        private readonly IDispatcher _dispatcher;

        public QueryPipelineTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLuigi(GetType().Assembly);
            
            var serviceProvider = serviceCollection.BuildServiceProvider();
            _dispatcher = serviceProvider.GetRequiredService<IDispatcher>();
        }
        
        [Fact]
        public void AddsToPipesList()
        {
            var pipeline = new HelloWorldQueryPipeline();
            var builder = new QueryPipelineBuilder<HelloWorldQuery, string>();
            pipeline.Configure(builder);
            builder.GetPipes().Length.ShouldBe(1);
        }

        [Fact]
        public async Task Should_throw_if_pipeline_not_registered()
        {
            var serviceCollection = new ServiceCollection();
            var dispatcher = new Dispatcher(serviceCollection.BuildServiceProvider());
            var ex = await dispatcher.ExecuteQuery<HelloWorldQuery, string>(new HelloWorldQuery()).ShouldThrowAsync<InvalidOperationException>();
            ex.Message.ShouldBe("No service for type 'Luigi.IQueryPipeline`2[Luigi.Tests.HelloWorldQuery,System.String]' has been registered.");
        }

        [Fact]
        public async Task Should_throw_if_pipe_not_registered()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<IQueryPipeline<HelloWorldQuery, string>, HelloWorldQueryPipeline>();
            var dispatcher = new Dispatcher(serviceCollection.BuildServiceProvider());
            var ex = await dispatcher.ExecuteQuery<HelloWorldQuery, string>(new HelloWorldQuery()).ShouldThrowAsync<InvalidOperationException>();
            ex.Message.ShouldBe($"No service for type '{typeof(HelloWorldQueryPipe).FullName}' has been registered.");
        }
        
        [Fact]
        public async Task Returns_response_from_PipelineContext()
        {
           var response = await _dispatcher.ExecuteQuery<HelloWorldQuery, string>(new HelloWorldQuery());
           response.ShouldBe("Hello World");
        }
        
        [Fact]
        public async Task Short_Circuit()
        { 
            var response = await _dispatcher.ExecuteQuery<ShortCircuitQuery, string>(new ShortCircuitQuery());
            response.ShouldBe("Short Circuit");
        }

        [Fact]
        public async Task WithContext()
        {
            var response = await _dispatcher.ExecuteQuery<HelloWorldWithContextQuery, string, HelloWorldContext>(new HelloWorldWithContextQuery());
            response.ShouldBe(new HelloWorldContext().Foo);
        }
    }
}