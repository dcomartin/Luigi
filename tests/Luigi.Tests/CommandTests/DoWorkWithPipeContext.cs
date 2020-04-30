using System;
using System.Threading.Tasks;

namespace Luigi.Tests
{
    public class DoWorkWithContextCommand : ICommand
    {
        
    }

    public class DoWorkContext
    {
        public string Foo { get; set; } = "Bar";
    } 
    
    public class DoWorkWithContextCommandPipeline : ICommandPipeline<DoWorkWithContextCommand, DoWorkContext>
    {
        public void Configure(ICommandPipelineBuilder<DoWorkWithContextCommand, DoWorkContext> builder)
        {
            builder.UsePipe<DoWorkWithContextCommandPipe>();
        }
    }
    
    public class DoWorkWithContextCommandPipe : ICommandPipe<DoWorkWithContextCommand, DoWorkContext>
    {
        public async Task Handle(CommandPipelineContext<DoWorkWithContextCommand, DoWorkContext> pipelineContext, Func<CommandPipelineContext<DoWorkWithContextCommand, DoWorkContext>, Task> next)
        {
            pipelineContext.PipeContext = new DoWorkContext();
            await next(pipelineContext);
        }
    }
}