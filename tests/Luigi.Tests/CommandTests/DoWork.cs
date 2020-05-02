using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luigi.Tests
{
    public class DoWorkCommand : ICommand
    {
        
    }
    
    public class DoWorkCommandPipeline : ICommandPipeline<DoWorkCommand>
    {
        public void Configure(ICommandPipelineBuilder<DoWorkCommand> builder)
        {
            builder.UsePipe<DoWorkCommandPipe>();
        }
    }
    
    public class DoWorkCommandPipe : ICommandPipe<DoWorkCommand>
    {
        public async Task Handle(CommandPipelineContext<DoWorkCommand> pipelineContext, Func<CommandPipelineContext<DoWorkCommand>, Task> next)
        {
            PipeVerify.PipesCalled.Add(GetType());
            await next(pipelineContext);
        }
    }
    
    public static class PipeVerify
    {
        public static List<Type> PipesCalled { get; set; } = new List<Type>();
    }
}