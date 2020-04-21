using System;
using System.Threading.Tasks;
using Luigi;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace TestProject1
{
    public class UnitTest1
    {
        private readonly IDispatcher _dispatcher;

        public UnitTest1()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLuigi(GetType().Assembly);
            
            var serviceProvider = serviceCollection.BuildServiceProvider();
            _dispatcher = serviceProvider.GetRequiredService<IDispatcher>();
        }
        
        [Fact]
        public void AddsToPipesList()
        {
            var pipeline = new HelloWorldPipeline();
            var builder = new PipelineBuilder<HelloWorldRequest, string>();
            pipeline.Configure(builder);
            builder.GetPipes().Length.ShouldBe(1);
        }

        [Fact]
        public async Task Should_throw_if_pipeline_not_registered()
        {
            var serviceCollection = new ServiceCollection();
            var dispatcher = new Dispatcher(serviceCollection.BuildServiceProvider());
            var ex = await dispatcher.Dispatch<HelloWorldRequest, string>(new HelloWorldRequest()).ShouldThrowAsync<InvalidOperationException>();
            ex.Message.ShouldBe("No service for type 'Luigi.IPipeline`2[TestProject1.HelloWorldRequest,System.String]' has been registered.");
        }

        [Fact]
        public async Task Should_throw_if_pipe_not_registered()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<IPipeline<HelloWorldRequest, string>, HelloWorldPipeline>();
            var dispatcher = new Dispatcher(serviceCollection.BuildServiceProvider());
            var ex = await dispatcher.Dispatch<HelloWorldRequest, string>(new HelloWorldRequest()).ShouldThrowAsync<InvalidOperationException>();
            ex.Message.ShouldBe("No service for type 'TestProject1.HelloWorldPipe' has been registered.");
        }
        
        [Fact]
        public async Task Returns_response_from_PipelineContext()
        {
           var response = await _dispatcher.Dispatch<HelloWorldRequest, string>(new HelloWorldRequest());
           response.ShouldBe("Hello World");
        }
        
        [Fact]
        public async Task Short_Circuit()
        { 
            var response = await _dispatcher.Dispatch<ShortCircuitRequest, string>(new ShortCircuitRequest());
            response.ShouldBe("Short Circuit");
        }
    }
}