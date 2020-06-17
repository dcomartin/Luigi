using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace Luigi
{
    public class HangfireDispatcher
    {
        private readonly IDispatcher _dispatcher;

        public HangfireDispatcher(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
        
        public async Task ExecuteCommand(ICommand command)
        {
            await _dispatcher.ExecuteCommand(command);
        }

        public async Task ExecuteEventPipeline(IEvent @event, Type eventPipelineType)
        {
            await _dispatcher.ExecuteEventPipeline(@event, eventPipelineType);
        }

        public async Task ExecuteEventHandler(IEvent @event, Type eventHandlerType)
        {
            await _dispatcher.ExecuteEventHandler(@event, eventHandlerType);
        }
    }

    public interface IQueueClient
    {
        Task<string> Enqueue(ICommand command);
    }
    
    public class HangfireQueueClient : IQueueClient
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IServiceProvider _serviceCollection;

        public HangfireQueueClient(IBackgroundJobClient backgroundJobClient, IServiceProvider serviceCollection)
        {
            _backgroundJobClient = backgroundJobClient;
            _serviceCollection = serviceCollection;
        }
        
        
        public Task<string> Enqueue(ICommand command)
        {
            var result = _backgroundJobClient.Enqueue<HangfireDispatcher>(dispatcher => dispatcher.ExecuteCommand(command));
            return Task.FromResult(result);
        }
        
        public Task<IEnumerable<string>> Enqueue<T>(T @event) where T : IEvent
        {
            var result = new List<string>();
            
            var eventType =  @event.GetType();
            if (_serviceCollection.GetService(typeof(IEventHandler<>).MakeGenericType(eventType)) is IEventHandler<T> eventHandler)
            {
                var jobId = _backgroundJobClient.Enqueue<HangfireDispatcher>(dispatcher => dispatcher.ExecuteEventHandler(@event, eventHandler.GetType()));
                result.Add(jobId);
            }
            
            var pipelines = _serviceCollection.GetServices(typeof(IEventPipeline<>).MakeGenericType(eventType));
            foreach (IEventPipeline<T> pipeline in pipelines)
            {
                var jobId = _backgroundJobClient.Enqueue<HangfireDispatcher>(dispatcher => dispatcher.ExecuteEventPipeline( @event, pipeline.GetType()));
                result.Add(jobId);
            }
            return Task.FromResult(result.AsEnumerable());
        }
    }

    public class LuigiHangfireJobActivator : JobActivator
    {
        private readonly IServiceProvider _serviceProvider;

        public LuigiHangfireJobActivator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override object ActivateJob(Type jobType)
        {
            if (jobType == typeof(HangfireDispatcher))
            {
                return new HangfireDispatcher(_serviceProvider.GetService<IDispatcher>());
            }

            return _serviceProvider.GetService(jobType);
        }
    }
}