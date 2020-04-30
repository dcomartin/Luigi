using System;
using System.Threading.Tasks;

namespace Luigi.Tests
{
    public class HelloWorldWithContextQuery : IQuery<string>
    {
        
    }

    public class HelloWorldContext
    {
        public string Foo { get; set; } = "Bar";
    } 
    
    public class HelloWorldWithContextQueryPipeline : IQueryPipeline<HelloWorldWithContextQuery, string, HelloWorldContext>
    {
        public void Configure(IQueryPipelineBuilder<HelloWorldWithContextQuery, string, HelloWorldContext> builder)
        {
            builder.UsePipe<HelloWorldWithContextQueryPipe>();
        }
    }
    
    public class HelloWorldWithContextQueryPipe : IQueryPipe<HelloWorldWithContextQuery, string, HelloWorldContext>
    {
        public async Task Handle(QueryPipelineContext<HelloWorldWithContextQuery, string, HelloWorldContext> queryPipelineContext, Func<QueryPipelineContext<HelloWorldWithContextQuery, string, HelloWorldContext>, Task> next)
        {
            queryPipelineContext.PipeContext = new HelloWorldContext();
            queryPipelineContext.Response = queryPipelineContext.PipeContext.Foo;
            await next(queryPipelineContext);
        }
    }
}