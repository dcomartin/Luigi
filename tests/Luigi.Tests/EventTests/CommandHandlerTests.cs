using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Luigi.Tests
{
    public class PingQuery : IQuery<string> { }

    public class PingQueryHandler : IQueryHandler<PingQuery, string>
    {
        public Task<string> Handle(PingQuery query)
        {
            return Task.FromResult("Pong");
        }
    }
    
    public class QueryHandlerTests
    {
        private readonly IDispatcher _dispatcher;

        public QueryHandlerTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLuigi(GetType().Assembly);
            
            var serviceProvider = serviceCollection.BuildServiceProvider();
            _dispatcher = serviceProvider.GetRequiredService<IDispatcher>();
        }

        [Fact]
        public async Task Should_call_handler()
        {
            var result = await _dispatcher.ExecuteQuery<PingQuery, string>(new PingQuery());
            result.ShouldBe("Pong");
        }
    }
}