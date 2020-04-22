using System;
using System.Threading.Tasks;

namespace Luigi.Tests
{
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
}