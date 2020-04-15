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
        private readonly ServiceProvider _serviceProvider;

        public UnitTest1()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient<IPipeline<HelloWorldRequest, string>, HelloWorldPipeline>();
            serviceCollection.AddTransient<HelloWorldPipe>();
            
            serviceCollection.AddTransient<IPipeline<ShortCircuitRequest, string>, ShortCircuitPipeline>();
            serviceCollection.AddTransient<ShortCircuitPipe>();
            serviceCollection.AddTransient<ShortCircuitNonReachablePipe>();
            
            _serviceProvider = serviceCollection.BuildServiceProvider();
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
            throw new NotImplementedException();
        }

        [Fact]
        public async Task Should_throw_if_pipe_not_registered()
        {
            throw new NotImplementedException();
        }
        
        [Fact]
        public async Task Returns_response_from_PipelineContext()
        {
           var dispatcher = new Dispatcher(_serviceProvider);
            var response = await dispatcher.Dispatch<HelloWorldRequest, string>(new HelloWorldRequest());
            response.ShouldBe("Hello World");
        }
        
        [Fact]
        public async Task Short_Circuit()
        { 
            var dispatcher = new Dispatcher(_serviceProvider);
            var response = await dispatcher.Dispatch<ShortCircuitRequest, string>(new ShortCircuitRequest());
            response.ShouldBe("Short Circuit");
        }
    }
    
    public class HelloWorldRequest 
    {
        
    }
    
    public class HelloWorldPipeline : IPipeline<HelloWorldRequest, string>
    {
        public void Configure(IPipelineBuilder<HelloWorldRequest, string> builder)
        {
            builder.UsePipe<HelloWorldPipe>();
        }
    }
    
    public class HelloWorldPipe : IPipe<HelloWorldRequest, string>
    {
        public async Task Handle(PipelineContext<HelloWorldRequest, string> pipelineContext, Func<PipelineContext<HelloWorldRequest, string>, Task> next)
        {
            pipelineContext.Response = "Hello World";
            await next(pipelineContext);
        }
    }
    
    public class ShortCircuitRequest
    {
        
    }
    
    public class ShortCircuitPipeline : IPipeline<ShortCircuitRequest, string>
    {
        public void Configure(IPipelineBuilder<ShortCircuitRequest, string> builder)
        {
            builder.UsePipe<ShortCircuitPipe>();
            builder.UsePipe<ShortCircuitNonReachablePipe>();
        }
    }
    
    public class ShortCircuitPipe : IPipe<ShortCircuitRequest, string>
    {
        public Task Handle(PipelineContext<ShortCircuitRequest, string> pipelineContext, Func<PipelineContext<ShortCircuitRequest, string>, Task> next)
        {
            pipelineContext.Response = "Short Circuit";
            return Task.CompletedTask;
        }
    }
    
    public class ShortCircuitNonReachablePipe : IPipe<ShortCircuitRequest, string>
    {
        public Task Handle(PipelineContext<ShortCircuitRequest, string> pipelineContext, Func<PipelineContext<ShortCircuitRequest, string>, Task> next)
        {
            pipelineContext.Response = "Non Reachable Pipe";
            return Task.CompletedTask;
        }
    }
}