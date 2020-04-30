using System;
using System.Threading.Tasks;

namespace Luigi
{
    public interface ICommandPipe<TRequest> where TRequest : ICommand
    {
        Task Handle(CommandPipelineContext<TRequest> commandPipelineContext, Func<CommandPipelineContext<TRequest>, Task> next);
    }
    
    public interface ICommandPipe<TRequest, TPipeContext> where TRequest : ICommand
    {
        Task Handle(CommandPipelineContext<TRequest, TPipeContext> commandPipelineContext, Func<CommandPipelineContext<TRequest, TPipeContext>, Task> next);
    }
}