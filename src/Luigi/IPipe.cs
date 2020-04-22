using System;
using System.Threading.Tasks;

namespace Luigi
{
    public interface IPipe<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task Handle(PipelineContext<TRequest, TResponse> pipelineContext, Func<PipelineContext<TRequest, TResponse>, Task> next);
    }
    
    public interface IPipe<TRequest, TResponse, TPipeContext> where TRequest : IRequest<TResponse>
    {
        Task Handle(PipelineContext<TRequest, TResponse, TPipeContext> pipelineContext, Func<PipelineContext<TRequest, TResponse, TPipeContext>, Task> next);
    }
}