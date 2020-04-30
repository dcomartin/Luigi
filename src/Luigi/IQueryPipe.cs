using System;
using System.Threading.Tasks;

namespace Luigi
{
    public interface IQueryPipe<TRequest, TResponse> where TRequest : IQuery<TResponse>
    {
        Task Handle(QueryPipelineContext<TRequest, TResponse> queryPipelineContext, Func<QueryPipelineContext<TRequest, TResponse>, Task> next);
    }
    
    public interface IQueryPipe<TRequest, TResponse, TPipeContext> where TRequest : IQuery<TResponse>
    {
        Task Handle(QueryPipelineContext<TRequest, TResponse, TPipeContext> queryPipelineContext, Func<QueryPipelineContext<TRequest, TResponse, TPipeContext>, Task> next);
    }
}