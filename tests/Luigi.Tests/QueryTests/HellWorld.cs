using System;
using System.Threading.Tasks;

namespace Luigi.Tests
{
    public class HelloWorldQuery : IQuery<string>
    {
        
    }
    
    public class HelloWorldQueryPipeline : IQueryPipeline<HelloWorldQuery, string>
    {
        public void Configure(IQueryPipelineBuilder<HelloWorldQuery, string> builder)
        {
            builder.UsePipe<HelloWorldQueryPipe>();
        }
    }
    
    public class HelloWorldQueryPipe : IQueryPipe<HelloWorldQuery, string>
    {
        public async Task Handle(QueryPipelineContext<HelloWorldQuery, string> queryPipelineContext, Func<QueryPipelineContext<HelloWorldQuery, string>, Task> next)
        {
            queryPipelineContext.Response = "Hello World";
            await next(queryPipelineContext);
        }
    }
}