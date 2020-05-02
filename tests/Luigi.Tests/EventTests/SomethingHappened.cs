using System;
using System.Threading.Tasks;

namespace Luigi.Tests.EventTests
{
    public class SomethingHappened : IEvent
    {
        
    }

    public class ReactToSomethingHappened1Pipeline : IEventPipeline<SomethingHappened>
    {
        public void Configure(IEventPipelineBuilder<SomethingHappened> builder)
        {
            builder.UsePipe<SomethingHappenedPipe1>();
        }
    }

    public class SomethingHappenedPipe1 : IEventPipe<SomethingHappened>
    {
        public Task Handle(EventPipelineContext<SomethingHappened> context, Func<EventPipelineContext<SomethingHappened>, Task> next)
        {
            PipeVerify.PipesCalled.Add(GetType());
            return next(context);
        }
    }
    
    public class ReactToSomethingHappened2Pipeline : IEventPipeline<SomethingHappened>
    {
        public void Configure(IEventPipelineBuilder<SomethingHappened> builder)
        {
            builder.UsePipe<SomethingHappenedPipe2>();
        }
    }

    public class SomethingHappenedPipe2 : IEventPipe<SomethingHappened>
    {
        public Task Handle(EventPipelineContext<SomethingHappened> context, Func<EventPipelineContext<SomethingHappened>, Task> next)
        {
            PipeVerify.PipesCalled.Add(GetType());
            return next(context);
        }
    }

    public class SomethingHappenedHandler : IEventHandler<SomethingHappened>
    {
        public Task Handle(SomethingHappened @event)
        {
            PipeVerify.PipesCalled.Add(GetType());
            return Task.CompletedTask;
        }
    }
}