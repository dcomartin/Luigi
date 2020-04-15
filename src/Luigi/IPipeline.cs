namespace Luigi
{
    public interface IPipeline<TRequest, TResponse>
    {
        void Configure(IPipelineBuilder<TRequest, TResponse> builder);
    }
}