using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luigi
{
    public interface ICommand { }
    
    public class CommandPipelineContext<TRequest>
    {
        public TRequest Request { get; set;  }
        
        public CommandPipelineContext(TRequest request)
        {
            Request = request;
        }
    }
    
    public class CommandPipelineContext<TRequest, TPipeContext>
    {
        public TRequest Request { get; set;  }
        public TPipeContext PipeContext { get; set; }

        public CommandPipelineContext(TRequest request, TPipeContext pipeContext = default)
        {
            Request = request;
            PipeContext = pipeContext;
        }
    }
    
    public interface ICommandPipelineBuilder<TRequest> where TRequest : ICommand
    {
        void UsePipe<TPipe>() where TPipe : ICommandPipe<TRequest>;
    }
    
    public interface ICommandPipelineBuilder<TRequest, TPipeContext> where TRequest : ICommand
    {
        void UsePipe<TPipe>() where TPipe : ICommandPipe<TRequest, TPipeContext>;
    }
    
    public class CommandPipelineBuilder<TRequest> : ICommandPipelineBuilder<TRequest> where TRequest : ICommand
    {
        private readonly List<Type> _pipes = new List<Type>();
        
        public void UsePipe<TPipe>() where TPipe : ICommandPipe<TRequest>
        {
            _pipes.Add(typeof(TPipe));
        }

        public Type[] GetPipes()
        {
            return _pipes.ToArray();
        }
    }
    
    public class CommandPipelineBuilder<TRequest, TPipeContext> : ICommandPipelineBuilder<TRequest, TPipeContext> where TRequest : ICommand
    {
        private readonly List<Type> _pipes = new List<Type>();
        
        public void UsePipe<TPipe>() where TPipe : ICommandPipe<TRequest, TPipeContext>
        {
            _pipes.Add(typeof(TPipe));
        }

        public Type[] GetPipes()
        {
            return _pipes.ToArray();
        }
    }

    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task Handle(TCommand command);
    }
    
    public interface ICommandPipeline<TRequest> where TRequest : ICommand
    {
        void Configure(ICommandPipelineBuilder<TRequest> builder);
    }
    
    public interface ICommandPipeline<TRequest, TPipeContext> where TRequest : ICommand
    {
        void Configure(ICommandPipelineBuilder<TRequest, TPipeContext> builder);
    }
    
    public interface ICommandPipe<TRequest> where TRequest : ICommand
    {
        Task Handle(CommandPipelineContext<TRequest> commandPipelineContext, Func<CommandPipelineContext<TRequest>, Task> next);
    }
    
    public interface ICommandPipe<TRequest, TPipeContext> where TRequest : ICommand
    {
        Task Handle(CommandPipelineContext<TRequest, TPipeContext> commandPipelineContext, Func<CommandPipelineContext<TRequest, TPipeContext>, Task> next);
    }
}