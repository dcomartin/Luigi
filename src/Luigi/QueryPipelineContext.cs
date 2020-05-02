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
    
    public class CommandPipelineContext<TRequest>
    {
        public TRequest Request { get; set;  }
        
        public CommandPipelineContext(TRequest request)
        {
            Request = request;
        }
    }
    
    public class CommandPipelineContext<TRequest, TPipeContext>
    {
        public TRequest Request { get; set;  }
        public TPipeContext PipeContext { get; set; }

        public CommandPipelineContext(TRequest request, TPipeContext pipeContext = default)
        {
            Request = request;
            PipeContext = pipeContext;
        }
    }
    
    public class EventPipelineContext<TEvent>
    {
        public TEvent Event { get; set;  }
        
        public EventPipelineContext(TEvent request)
        {
            Event = request;
        }
    }
    
    public class EventPipelineContext<TEvent, TPipeContext>
    {
        public TEvent Event { get; set;  }
        public TPipeContext PipeContext { get; set; }

        public EventPipelineContext(TEvent request, TPipeContext pipeContext = default)
        {
            Event = request;
            PipeContext = pipeContext;
        }
    }
}