using System;
using System.Threading.Tasks;

namespace Luigi.Tests
{
    public class HelloWorldWithContextRequest : IRequest<string>
    {
        
    }

    public class HelloWorldContext
    {
        public string Foo { get; set; } = "Bar";
    } 
    
    public class HelloWorldWithContextPipeline : IPipeline<HelloWorldWithContextRequest, string, HelloWorldContext>
    {
        public void Configure(IPipelineBuilder<HelloWorldWithContextRequest, string, HelloWorldContext> builder)
        {
            builder.UsePipe<HelloWorldWithContextPipe>();
        }
    }
    
    public class HelloWorldWithContextPipe : IPipe<HelloWorldWithContextRequest, string, HelloWorldContext>
    {
        public async Task Handle(PipelineContext<HelloWorldWithContextRequest, string, HelloWorldContext> pipelineContext, Func<PipelineContext<HelloWorldWithContextRequest, string, HelloWorldContext>, Task> next)
        {
            pipelineContext.PipeContext = new HelloWorldContext();
            pipelineContext.Response = pipelineContext.PipeContext.Foo;
            await next(pipelineContext);
        }
    }
}