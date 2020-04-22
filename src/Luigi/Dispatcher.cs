using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Luigi
{
    public interface IDispatcher
    {
        Task<TResponse> Dispatch<TRequest, TResponse>(TRequest request) where TRequest : IRequest<TResponse>;
        Task<TResponse> Dispatch<TRequest, TResponse, TPipeContext>(TRequest request) where TRequest : IRequest<TResponse>;
    }
    
    public interface IRequest<TResponse> { }
    
    public class Dispatcher : IDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public Dispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        private async Task ExecutePipeline<TRequest, TResponse>(Type[] pipes, PipelineContext<TRequest, TResponse> pipelineContext) where TRequest : IRequest<TResponse>
        {
            if (pipes.Any() == false)
            {
                return;
            }

            var pipeType = pipes.First(); 
            var pipeObj = _serviceProvider.GetRequiredService(pipeType) as IPipe<TRequest, TResponse>;

            if (pipeObj == null)
            {
                throw new InvalidOperationException($"{pipeType.FullName} is not a valid IPipe");
            }
            
            await pipeObj.Handle(pipelineContext, async (p2) =>
            {
                if (pipes.Length > 1)
                {
                    await ExecutePipeline(pipes.Skip(1).ToArray(), p2);
                }
            });
        }
        
        private async Task ExecutePipeline<TRequest, TResponse, TContext>(Type[] pipes, PipelineContext<TRequest, TResponse, TContext> pipelineContext) where TRequest : IRequest<TResponse>
        {
            if (pipes.Any() == false)
            {
                return;
            }

            var pipeType = pipes.First(); 
            var pipeObj = _serviceProvider.GetRequiredService(pipeType) as IPipe<TRequest, TResponse, TContext>;

            if (pipeObj == null)
            {
                throw new InvalidOperationException($"{pipeType.FullName} is not a valid IPipe");
            }
            
            await pipeObj.Handle(pipelineContext, async (p2) =>
            {
                if (pipes.Length > 1)
                {
                    await ExecutePipeline(pipes.Skip(1).ToArray(), p2);
                }
            });
        }
        
        public async Task<TResponse> Dispatch<TRequest, TResponse>(TRequest request) where TRequest : IRequest<TResponse>
        {
            var pipeContext = new PipelineContext<TRequest, TResponse>(request);
            
            var builder = new PipelineBuilder<TRequest, TResponse>();
            
            var pipeline = _serviceProvider.GetRequiredService(typeof(IPipeline<TRequest, TResponse>)) as IPipeline<TRequest, TResponse>;
            if (pipeline == null)
            {
                throw new InvalidOperationException($"Pipeline does not exist for {request.GetType().FullName}");
            }
            
            pipeline.Configure(builder);
            
            await ExecutePipeline(builder.GetPipes(), pipeContext);

            return pipeContext.Response;
        }

        public async Task<TResponse> Dispatch<TRequest, TResponse, TPipeContext>(TRequest request) where TRequest : IRequest<TResponse>
        {
            var pipeContext = new PipelineContext<TRequest, TResponse, TPipeContext>(request);
            
            var builder = new PipelineBuilder<TRequest, TResponse, TPipeContext>();
            
            var pipeline = _serviceProvider.GetRequiredService(typeof(IPipeline<TRequest, TResponse, TPipeContext>)) as IPipeline<TRequest, TResponse, TPipeContext>;
            if (pipeline == null)
            {
                throw new InvalidOperationException($"Pipeline does not exist for {request.GetType().FullName}");
            }
            
            pipeline.Configure(builder);
            
            await ExecutePipeline(builder.GetPipes(), pipeContext);

            return pipeContext.Response;
        }
    }
    
    
}