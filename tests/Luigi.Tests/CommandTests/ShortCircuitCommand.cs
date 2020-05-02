using System;
using System.Threading.Tasks;

namespace Luigi.Tests
{
    public class ShortCircuitCommand : ICommand
    {
        
    }
    
    public class ShortCircuitCommandPipeline : ICommandPipeline<ShortCircuitCommand>
    {
        public void Configure(ICommandPipelineBuilder<ShortCircuitCommand> builder)
        {
            builder.UsePipe<ShortCircuitCommandPipe>();
            builder.UsePipe<ShortCircuitNonReachableCommandPipe>();
        }
    }
    
    public class ShortCircuitCommandPipe : ICommandPipe<ShortCircuitCommand>
    {
        public Task Handle(CommandPipelineContext<ShortCircuitCommand> pipelineContext, Func<CommandPipelineContext<ShortCircuitCommand>, Task> next)
        {
            PipeVerify.PipesCalled.Add(this.GetType());
            return Task.CompletedTask;
        }
    }
    
    public class ShortCircuitNonReachableCommandPipe : ICommandPipe<ShortCircuitCommand>
    {
        public Task Handle(CommandPipelineContext<ShortCircuitCommand> pipelineContext, Func<CommandPipelineContext<ShortCircuitCommand>, Task> next)
        {
            PipeVerify.PipesCalled.Add(this.GetType());
            return next(pipelineContext);
        }
    }
}