using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luigi
{
    public class EventPipelineContext<TEvent>
    {
        public TEvent Event { get; set;  }
        
        public EventPipelineContext(TEvent request)
        {
            Event = request;
        }
    }
    
    public class EventPipelineContext<TEvent, TPipeContext>
    {
        public TEvent Event { get; set;  }
        public TPipeContext PipeContext { get; set; }

        public EventPipelineContext(TEvent request, TPipeContext pipeContext = default)
        {
            Event = request;
            PipeContext = pipeContext;
        }
    }
    
    public interface IEventPipelineBuilder<TEvent> where TEvent : IEvent
    {
        void UsePipe<TPipe>() where TPipe : IEventPipe<TEvent>;
    }
    
    public interface IEventPipelineBuilder<TEvent, TPipeContext> where TEvent : IEvent
    {
        void UsePipe<TPipe>() where TPipe : IEventPipe<TEvent, TPipeContext>;
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
    
    public interface IEventPipeline<TEvent> where TEvent : IEvent
    {
        void Configure(IEventPipelineBuilder<TEvent> builder);
    }
    
    public interface IEventPipeline<TEvent, TPipeContext> where TEvent : IEvent
    {
        void Configure(IEventPipelineBuilder<TEvent, TPipeContext> builder);
    }
    
    public interface IEventPipe<TEvent> where TEvent : IEvent
    {
        Task Handle(EventPipelineContext<TEvent> context, Func<EventPipelineContext<TEvent>, Task> next);
    }
    
    public interface IEventPipe<TEvent, TPipeContext> where TEvent : IEvent
    {
        Task Handle(EventPipelineContext<TEvent, TPipeContext> context, Func<EventPipelineContext<TEvent, TPipeContext>, Task> next);
    }
}