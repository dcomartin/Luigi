using System;
using System.Threading.Tasks;

namespace Luigi
{
    public interface IEventPipe<TEvent> where TEvent : IEvent
    {
        Task Handle(EventPipelineContext<TEvent> context, Func<EventPipelineContext<TEvent>, Task> next);
    }
    
    public interface IEventPipe<TEvent, TPipeContext> where TEvent : IEvent
    {
        Task Handle(EventPipelineContext<TEvent, TPipeContext> context, Func<EventPipelineContext<TEvent, TPipeContext>, Task> next);
    }
}