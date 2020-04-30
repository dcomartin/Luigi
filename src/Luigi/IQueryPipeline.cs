namespace Luigi
{
    public interface IQueryPipeline<TRequest, TResponse> where TRequest : IQuery<TResponse>
    {
        void Configure(IQueryPipelineBuilder<TRequest, TResponse> builder);
    }
    
    public interface IQueryPipeline<TRequest, TResponse, TPipeContext> where TRequest : IQuery<TResponse>
    {
        void Configure(IQueryPipelineBuilder<TRequest, TResponse, TPipeContext> builder);
    }
    
    public interface ICommandPipeline<TRequest> where TRequest : ICommand
    {
        void Configure(ICommandPipelineBuilder<TRequest> builder);
    }
    
    public interface ICommandPipeline<TRequest, TPipeContext> where TRequest : ICommand
    {
        void Configure(ICommandPipelineBuilder<TRequest, TPipeContext> builder);
    }
}