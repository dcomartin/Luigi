using System;
using System.Collections.Generic;

namespace Luigi
{
    public interface IQueryPipelineBuilder<TRequest, TResponse> where TRequest : IQuery<TResponse>
    {
        void UsePipe<TPipe>() where TPipe : IQueryPipe<TRequest, TResponse>;
    }
    
    public interface IQueryPipelineBuilder<TRequest, TResponse, TPipeContext> where TRequest : IQuery<TResponse>
    {
        void UsePipe<TPipe>()where TPipe : IQueryPipe<TRequest, TResponse, TPipeContext>;
    }
    
    public interface ICommandPipelineBuilder<TRequest> where TRequest : ICommand
    {
        void UsePipe<TPipe>() where TPipe : ICommandPipe<TRequest>;
    }
    
    public interface ICommandPipelineBuilder<TRequest, TPipeContext> where TRequest : ICommand
    {
        void UsePipe<TPipe>() where TPipe : ICommandPipe<TRequest, TPipeContext>;
    }
    
    public interface IEventPipelineBuilder<TEvent> where TEvent : IEvent
    {
        void UsePipe<TPipe>() where TPipe : IEventPipe<TEvent>;
    }
    
    public interface IEventPipelineBuilder<TEvent, TPipeContext> where TEvent : IEvent
    {
        void UsePipe<TPipe>() where TPipe : IEventPipe<TEvent, TPipeContext>;
    }

    public class QueryPipelineBuilder<TRequest, TResponse> : IQueryPipelineBuilder<TRequest, TResponse> where TRequest : IQuery<TResponse>
    {
        private readonly List<Type> _pipes = new List<Type>();
        
        public void UsePipe<TPipe>() where TPipe : IQueryPipe<TRequest, TResponse>
        {
            _pipes.Add(typeof(TPipe));
        }

        public Type[] GetPipes()
        {
            return _pipes.ToArray();
        }
    }
    
    public class QueryPipelineBuilder<TRequest, TResponse, TPipeContext> : IQueryPipelineBuilder<TRequest, TResponse, TPipeContext> where TRequest : IQuery<TResponse>
    {
        private readonly List<Type> _pipes = new List<Type>();
        
        public void UsePipe<TPipe>() where TPipe : IQueryPipe<TRequest, TResponse, TPipeContext>
        {
            _pipes.Add(typeof(TPipe));
        }

        public Type[] GetPipes()
        {
            return _pipes.ToArray();
        }
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
    
    public class EventPipelineBuilder<TEvent> : IEventPipelineBuilder<TEvent> where TEvent : IEvent
    {
        private readonly List<Type> _pipes = new List<Type>();
        
        public void UsePipe<TPipe>() where TPipe : IEventPipe<TEvent>
        {
            _pipes.Add(typeof(TPipe));
        }

        public Type[] GetPipes()
        {
            return _pipes.ToArray();
        }
    }
    
    public class EventPipelineBuilder<TEvent, TPipeContext> : IEventPipelineBuilder<TEvent, TPipeContext> where TEvent : IEvent
    {
        private readonly List<Type> _pipes = new List<Type>();
        
        public void UsePipe<TPipe>() where TPipe : IEventPipe<TEvent, TPipeContext>
        {
            _pipes.Add(typeof(TPipe));
        }

        public Type[] GetPipes()
        {
            return _pipes.ToArray();
        }
    }
}