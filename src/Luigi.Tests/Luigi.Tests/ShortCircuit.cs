using System;
using System.Threading.Tasks;
using Luigi;

namespace TestProject1
{
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