using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Luigi.Tests
{
    public class DoMoreWork : ICommand { }

    public class DoMoreWorkHandler : ICommandHandler<DoMoreWork>
    {
        public static bool WasCalled = false;
        
        public Task Handle(DoMoreWork command)
        {
            WasCalled = true;
            return Task.CompletedTask;
        }
    }
    
    public class CommandHandlerTests
    {
        private readonly IDispatcher _dispatcher;

        public CommandHandlerTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLuigi(GetType().Assembly);
            
            var serviceProvider = serviceCollection.BuildServiceProvider();
            _dispatcher = serviceProvider.GetRequiredService<IDispatcher>();
        }

        [Fact]
        public async Task Should_call_handler()
        {
            await _dispatcher.ExecuteCommand(new DoMoreWork());
            DoMoreWorkHandler.WasCalled.ShouldBeTrue();
        }
    }
}