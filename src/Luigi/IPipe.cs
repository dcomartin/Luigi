using System;
using System.Threading.Tasks;

namespace Luigi
{
    public interface IPipe<TRequest, TResponse>
    {
        Task Handle(PipelineContext<TRequest, TResponse> pipelineContext, Func<PipelineContext<TRequest, TResponse>, Task> next);
    }
}