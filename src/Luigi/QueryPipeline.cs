using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Luigi
{
    public class QueryPipelineContext<TRequest, TResponse>
    {
        public TRequest Request { get; set;  }
        public TResponse Response { get; set; }
        
        public QueryPipelineContext(TRequest request, TResponse response = default)
        {
            Request = request;
            Response = response;
        }
    }
    
    public class QueryPipelineContext<TRequest, TResponse, TPipeContext>
    {
        public TRequest Request { get; set;  }
        public TResponse Response { get; set; }
        public TPipeContext PipeContext { get; set; }
        
        public QueryPipelineContext(TRequest request, TResponse response = default, TPipeContext pipeContext = default)
        {
            Request = request;
            Response = response;
            PipeContext = pipeContext;
        }
    }
    
    public class QueryPipelineBuilder<TRequest, TResponse> : IQueryPipelineBuilder<TRequest, TResponse> where TRequest : IQuery<TResponse>
    {
        private readonly List<Type> _pipes = new List<Type>();
        
        public void UsePipe<TPipe>() where TPipe : IQueryPipe<TRequest, TResponse>
        {
            _pipes.Add(typeof(TPipe));
        }

        public Type[] GetPipes()
        {
            return _pipes.ToArray();
        }
    }
    
    public class QueryPipelineBuilder<TRequest, TResponse, TPipeContext> : IQueryPipelineBuilder<TRequest, TResponse, TPipeContext> where TRequest : IQuery<TResponse>
    {
        private readonly List<Type> _pipes = new List<Type>();
        
        public void UsePipe<TPipe>() where TPipe : IQueryPipe<TRequest, TResponse, TPipeContext>
        {
            _pipes.Add(typeof(TPipe));
        }

        public Type[] GetPipes()
        {
            return _pipes.ToArray();
        }
    }
    
    public interface IQueryPipelineBuilder<TRequest, TResponse> where TRequest : IQuery<TResponse>
    {
        void UsePipe<TPipe>() where TPipe : IQueryPipe<TRequest, TResponse>;
    }
    
    public interface IQueryPipelineBuilder<TRequest, TResponse, TPipeContext> where TRequest : IQuery<TResponse>
    {
        void UsePipe<TPipe>()where TPipe : IQueryPipe<TRequest, TResponse, TPipeContext>;
    }
    
    public interface IQueryPipeline<TRequest, TResponse> where TRequest : IQuery<TResponse>
    {
        void Configure(IQueryPipelineBuilder<TRequest, TResponse> builder);
    }
    
    public interface IQueryPipeline<TRequest, TResponse, TPipeContext> where TRequest : IQuery<TResponse>
    {
        void Configure(IQueryPipelineBuilder<TRequest, TResponse, TPipeContext> builder);
    }
    
    public interface IQueryPipe<TRequest, TResponse> where TRequest : IQuery<TResponse>
    {
        Task Handle(QueryPipelineContext<TRequest, TResponse> queryPipelineContext, Func<QueryPipelineContext<TRequest, TResponse>, Task> next);
    }
    
    public interface IQueryPipe<TRequest, TResponse, TPipeContext> where TRequest : IQuery<TResponse>
    {
        Task Handle(QueryPipelineContext<TRequest, TResponse, TPipeContext> queryPipelineContext, Func<QueryPipelineContext<TRequest, TResponse, TPipeContext>, Task> next);
    }
}