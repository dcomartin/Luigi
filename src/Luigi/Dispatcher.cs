using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Luigi
{
    public interface IDispatcher
    {
        Task<TResponse> DispatchQuery<TRequest, TResponse>(TRequest request) where TRequest : IQuery<TResponse>;
        Task<TResponse> DispatchQuery<TRequest, TResponse, TPipeContext>(TRequest request) where TRequest : IQuery<TResponse>;
        Task DispatchCommand<TRequest>(TRequest request) where TRequest : ICommand;
        Task DispatchCommand<TRequest, TPipeContext>(TRequest request) where TRequest : ICommand;
    }
    
    public interface ICommand { }
    public interface IQuery<TResponse> { }
    
    public class Dispatcher : IDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public Dispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        private async Task ExecutePipeline<TRequest, TResponse>(Type[] pipes, QueryPipelineContext<TRequest, TResponse> queryPipelineContext) where TRequest : IQuery<TResponse>
        {
            if (pipes.Any() == false)
            {
                return;
            }

            var pipeType = pipes.First(); 
            var pipeObj = _serviceProvider.GetRequiredService(pipeType) as IQueryPipe<TRequest, TResponse>;

            if (pipeObj == null)
            {
                throw new InvalidOperationException($"{pipeType.FullName} is not a valid IPipe");
            }
            
            await pipeObj.Handle(queryPipelineContext, async (p2) =>
            {
                if (pipes.Length > 1)
                {
                    await ExecutePipeline(pipes.Skip(1).ToArray(), p2);
                }
            });
        }
        
        private async Task ExecutePipeline<TRequest, TResponse, TContext>(Type[] pipes, QueryPipelineContext<TRequest, TResponse, TContext> queryPipelineContext) where TRequest : IQuery<TResponse>
        {
            if (pipes.Any() == false)
            {
                return;
            }

            var pipeType = pipes.First(); 
            var pipeObj = _serviceProvider.GetRequiredService(pipeType) as IQueryPipe<TRequest, TResponse, TContext>;

            if (pipeObj == null)
            {
                throw new InvalidOperationException($"{pipeType.FullName} is not a valid IPipe");
            }
            
            await pipeObj.Handle(queryPipelineContext, async (p2) =>
            {
                if (pipes.Length > 1)
                {
                    await ExecutePipeline(pipes.Skip(1).ToArray(), p2);
                }
            });
        }
        
        private async Task ExecutePipeline<TRequest>(Type[] pipes, CommandPipelineContext<TRequest> commandPipelineContext) where TRequest : ICommand
        {
            if (pipes.Any() == false)
            {
                return;
            }

            var pipeType = pipes.First(); 
            var pipeObj = _serviceProvider.GetRequiredService(pipeType) as ICommandPipe<TRequest>;

            if (pipeObj == null)
            {
                throw new InvalidOperationException($"{pipeType.FullName} is not a valid IPipe");
            }
            
            await pipeObj.Handle(commandPipelineContext, async (p2) =>
            {
                if (pipes.Length > 1)
                {
                    await ExecutePipeline(pipes.Skip(1).ToArray(), p2);
                }
            });
        }
        
        private async Task ExecutePipeline<TRequest, TContext>(Type[] pipes, CommandPipelineContext<TRequest, TContext> commandPipelineContext) where TRequest : ICommand
        {
            if (pipes.Any() == false)
            {
                return;
            }

            var pipeType = pipes.First(); 
            var pipeObj = _serviceProvider.GetRequiredService(pipeType) as ICommandPipe<TRequest, TContext>;

            if (pipeObj == null)
            {
                throw new InvalidOperationException($"{pipeType.FullName} is not a valid IPipe");
            }
            
            await pipeObj.Handle(commandPipelineContext, async (p2) =>
            {
                if (pipes.Length > 1)
                {
                    await ExecutePipeline(pipes.Skip(1).ToArray(), p2);
                }
            });
        }
        
        public async Task<TResponse> DispatchQuery<TRequest, TResponse>(TRequest request) where TRequest : IQuery<TResponse>
        {
            var pipeContext = new QueryPipelineContext<TRequest, TResponse>(request);
            
            var builder = new QueryPipelineBuilder<TRequest, TResponse>();
            
            var pipeline = _serviceProvider.GetRequiredService(typeof(IQueryPipeline<TRequest, TResponse>)) as IQueryPipeline<TRequest, TResponse>;
            if (pipeline == null)
            {
                throw new InvalidOperationException($"Pipeline does not exist for {request.GetType().FullName}");
            }
            
            pipeline.Configure(builder);
            
            await ExecutePipeline(builder.GetPipes(), pipeContext);

            return pipeContext.Response;
        }

        public async Task<TResponse> DispatchQuery<TRequest, TResponse, TPipeContext>(TRequest request) where TRequest : IQuery<TResponse>
        {
            var pipeContext = new QueryPipelineContext<TRequest, TResponse, TPipeContext>(request);
            
            var builder = new QueryPipelineBuilder<TRequest, TResponse, TPipeContext>();
            
            var pipeline = _serviceProvider.GetRequiredService(typeof(IQueryPipeline<TRequest, TResponse, TPipeContext>)) as IQueryPipeline<TRequest, TResponse, TPipeContext>;
            if (pipeline == null)
            {
                throw new InvalidOperationException($"Pipeline does not exist for {request.GetType().FullName}");
            }
            
            pipeline.Configure(builder);
            
            await ExecutePipeline(builder.GetPipes(), pipeContext);

            return pipeContext.Response;
        }

        public async Task DispatchCommand<TRequest>(TRequest request) where TRequest : ICommand
        {
            var pipeContext = new CommandPipelineContext<TRequest>(request);
            
            var builder = new CommandPipelineBuilder<TRequest>();
            
            var pipeline = _serviceProvider.GetRequiredService(typeof(ICommandPipeline<TRequest>)) as ICommandPipeline<TRequest>;
            if (pipeline == null)
            {
                throw new InvalidOperationException($"Pipeline does not exist for {request.GetType().FullName}");
            }
            
            pipeline.Configure(builder);
            
            await ExecutePipeline(builder.GetPipes(), pipeContext);
        }

        public async Task DispatchCommand<TRequest, TPipeContext>(TRequest request) where TRequest : ICommand
        {
            var pipeContext = new CommandPipelineContext<TRequest, TPipeContext>(request);
            
            var builder = new CommandPipelineBuilder<TRequest, TPipeContext>();
            
            var pipeline = _serviceProvider.GetRequiredService(typeof(ICommandPipeline<TRequest, TPipeContext>)) as ICommandPipeline<TRequest, TPipeContext>;
            if (pipeline == null)
            {
                throw new InvalidOperationException($"Pipeline does not exist for {request.GetType().FullName}");
            }
            
            pipeline.Configure(builder);
            
            await ExecutePipeline(builder.GetPipes(), pipeContext);
        }
    }
}