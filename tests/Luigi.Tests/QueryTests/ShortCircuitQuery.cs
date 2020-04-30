using System;
using System.Threading.Tasks;

namespace Luigi.Tests
{
    public class ShortCircuitQuery : IQuery<string>
    {
        
    }
    
    public class ShortCircuitQueryPipeline : IQueryPipeline<ShortCircuitQuery, string>
    {
        public void Configure(IQueryPipelineBuilder<ShortCircuitQuery, string> builder)
        {
            builder.UsePipe<ShortCircuitQueryPipe>();
            builder.UsePipe<ShortCircuitNonReachableQueryPipe>();
        }
    }
    
    public class ShortCircuitQueryPipe : IQueryPipe<ShortCircuitQuery, string>
    {
        public Task Handle(QueryPipelineContext<ShortCircuitQuery, string> queryPipelineContext, Func<QueryPipelineContext<ShortCircuitQuery, string>, Task> next)
        {
            queryPipelineContext.Response = "Short Circuit";
            return Task.CompletedTask;
        }
    }
    
    public class ShortCircuitNonReachableQueryPipe : IQueryPipe<ShortCircuitQuery, string>
    {
        public Task Handle(QueryPipelineContext<ShortCircuitQuery, string> queryPipelineContext, Func<QueryPipelineContext<ShortCircuitQuery, string>, Task> next)
        {
            queryPipelineContext.Response = "Non Reachable Pipe";
            return Task.CompletedTask;
        }
    }
}