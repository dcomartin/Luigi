using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Common;
using Hangfire.MemoryStorage;
using Hangfire.Storage.Monitoring;
using Luigi.Tests.EventTests;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Luigi.Tests
{
    public class HangfireQueueClientTests
    {
        [Fact]
        public async Task Should_enqueue_command()
        {
            var memoryStorage = new MemoryStorage();
            
            var serviceCollection = new ServiceCollection();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            GlobalConfiguration.Configuration.UseLuigi(serviceProvider);
            GlobalConfiguration.Configuration.UseStorage(memoryStorage);
            
            var sut = new HangfireQueueClient(new BackgroundJobClient(), serviceProvider);
            var jobId = await sut.Enqueue(new DoWorkCommand());
            
            var job = memoryStorage.GetMonitoringApi().JobDetails(jobId);
            job.ShouldNotBeNull();
            job.Job.Type.ShouldBe(typeof(HangfireDispatcher));
        }
        
        [Fact]
        public async Task Should_enqueue_event_handlers()
        {
            var memoryStorage = new MemoryStorage();
            
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLuigi(GetType().Assembly);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            GlobalConfiguration.Configuration.UseLuigi(serviceProvider);
            GlobalConfiguration.Configuration.UseStorage(memoryStorage);
            
            var sut = new HangfireQueueClient(new BackgroundJobClient(), serviceProvider);
            var jobIds = await sut.Enqueue(new SomethingHappened());

            jobIds.Count().ShouldBe(3);
            
            var monitoring = memoryStorage.GetMonitoringApi();

            var jobs = new List<JobDetailsDto>();
            foreach (var jobId in jobIds)
            {
                var job = monitoring.JobDetails(jobId);
                jobs.Add(job);
            }

            var handler = jobs.SingleOrDefault(x => (Type) x.Job.Args.Last() == typeof(SomethingHappenedHandler));
            handler.ShouldNotBeNull();
            handler.Job.Args.First().GetType().ShouldBe(typeof(SomethingHappened));
            
            var pipeline1 = jobs.SingleOrDefault(x => (Type) x.Job.Args.Last() == typeof(ReactToSomethingHappened1Pipeline));
            pipeline1.ShouldNotBeNull();
            pipeline1.Job.Args.First().GetType().ShouldBe(typeof(SomethingHappened));
            
            var pipeline2 = jobs.SingleOrDefault(x => (Type) x.Job.Args.Last() == typeof(ReactToSomethingHappened2Pipeline));
            pipeline2.ShouldNotBeNull();
            pipeline2.Job.Args.First().GetType().ShouldBe(typeof(SomethingHappened));
        }
    }
}