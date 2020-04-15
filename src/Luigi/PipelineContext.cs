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
}