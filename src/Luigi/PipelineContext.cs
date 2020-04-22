namespace Luigi
{
    public class PipelineContext<TRequest, TResponse>
    {
        public TRequest Request { get; set;  }
        public TResponse Response { get; set; }
        
        public PipelineContext(TRequest request, TResponse response = default)
        {
            Request = request;
            Response = response;
        }
    }
    
    public class PipelineContext<TRequest, TResponse, TPipeContext>
    {
        public TRequest Request { get; set;  }
        public TResponse Response { get; set; }
        public TPipeContext PipeContext { get; set; }
        
        public PipelineContext(TRequest request, TResponse response = default, TPipeContext pipeContext = default)
        {
            Request = request;
            Response = response;
            PipeContext = pipeContext;
        }
    }
}