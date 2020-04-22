namespace Luigi
{
    public interface IPipeline<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        void Configure(IPipelineBuilder<TRequest, TResponse> builder);
    }
    
    public interface IPipeline<TRequest, TResponse, TPipeContext> where TRequest : IRequest<TResponse>
    {
        void Configure(IPipelineBuilder<TRequest, TResponse, TPipeContext> builder);
    }
}