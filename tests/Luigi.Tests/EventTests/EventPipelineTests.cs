using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Luigi.Tests.EventTests
{
    public class EventPipelineTests
    {
        private readonly IDispatcher _dispatcher;

        public EventPipelineTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLuigi(GetType().Assembly);
            
            var serviceProvider = serviceCollection.BuildServiceProvider();
            _dispatcher = serviceProvider.GetRequiredService<IDispatcher>();
        }
        
        [Fact]
        public async Task Should_call_all_handlers()
        {
            await _dispatcher.PublishEvent(new SomethingHappened());
            PipeVerify.PipesCalled.ShouldContain(x => x == typeof(SomethingHappenedPipe1));
            PipeVerify.PipesCalled.ShouldContain(x => x == typeof(SomethingHappenedPipe2));
        }
    }
}