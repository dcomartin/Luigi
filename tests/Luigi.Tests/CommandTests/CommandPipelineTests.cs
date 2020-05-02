using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Luigi.Tests
{
    public class CommandPipelineTests
    {
        private readonly IDispatcher _dispatcher;

        public CommandPipelineTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLuigi(GetType().Assembly);
            
            var serviceProvider = serviceCollection.BuildServiceProvider();
            _dispatcher = serviceProvider.GetRequiredService<IDispatcher>();
        }
        
        [Fact]
        public void AddsToPipesList()
        {
            var pipeline = new DoWorkCommandPipeline();
            var builder = new CommandPipelineBuilder<DoWorkCommand>();
            pipeline.Configure(builder);
            builder.GetPipes().Length.ShouldBe(1);
        }

        [Fact]
        public async Task Should_throw_if_pipeline_not_registered()
        {
            var serviceCollection = new ServiceCollection();
            var dispatcher = new Dispatcher(serviceCollection.BuildServiceProvider());
            var ex = await dispatcher.ExecuteCommand<DoWorkCommand>(new DoWorkCommand()).ShouldThrowAsync<InvalidOperationException>();
            ex.Message.ShouldBe($"No service for type 'Luigi.ICommandPipeline`1[Luigi.Tests.DoWorkCommand]' has been registered.");
        }

        [Fact]
        public async Task Should_throw_if_pipe_not_registered()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<ICommandPipeline<DoWorkCommand>, DoWorkCommandPipeline>();
            var dispatcher = new Dispatcher(serviceCollection.BuildServiceProvider());
            var ex = await dispatcher.ExecuteCommand<DoWorkCommand>(new DoWorkCommand()).ShouldThrowAsync<InvalidOperationException>();
            ex.Message.ShouldBe($"No service for type '{typeof(DoWorkCommandPipe).FullName}' has been registered.");
        }
        
        [Fact]
        public async Task Executes_all_pipes_PipelineContext()
        {
           await _dispatcher.ExecuteCommand<DoWorkCommand>(new DoWorkCommand());
           PipeVerify.PipesCalled.ShouldContain(x => x == typeof(DoWorkCommandPipe));
        }
        
        [Fact]
        public async Task Short_Circuit()
        { 
            await _dispatcher.ExecuteCommand<ShortCircuitCommand>(new ShortCircuitCommand());
            PipeVerify.PipesCalled.ShouldNotContain(x => x == typeof(ShortCircuitNonReachableCommandPipe));
        }

        [Fact]
        public async Task WithContext()
        {
            await _dispatcher.ExecuteCommand<DoWorkWithContextCommand, DoWorkContext>(new DoWorkWithContextCommand());
        }
    }
}