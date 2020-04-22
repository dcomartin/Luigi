using System;
using System.Collections.Generic;

namespace Luigi
{
    public interface IPipelineBuilder<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        void UsePipe<TPipe>() where TPipe : IPipe<TRequest, TResponse>;
    }
    
    public interface IPipelineBuilder<TRequest, TResponse, TPipeContext> where TRequest : IRequest<TResponse>
    {
        void UsePipe<TPipe>()where TPipe : IPipe<TRequest, TResponse, TPipeContext>;
    }

    public class PipelineBuilder<TRequest, TResponse> : IPipelineBuilder<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly List<Type> _pipes = new List<Type>();
        
        public void UsePipe<TPipe>() where TPipe : IPipe<TRequest, TResponse>
        {
            _pipes.Add(typeof(TPipe));
        }

        public Type[] GetPipes()
        {
            return _pipes.ToArray();
        }
    }
    
    public class PipelineBuilder<TRequest, TResponse, TPipeContext> : IPipelineBuilder<TRequest, TResponse, TPipeContext> where TRequest : IRequest<TResponse>
    {
        private readonly List<Type> _pipes = new List<Type>();
        
        public void UsePipe<TPipe>() where TPipe : IPipe<TRequest, TResponse, TPipeContext>
        {
            _pipes.Add(typeof(TPipe));
        }

        public Type[] GetPipes()
        {
            return _pipes.ToArray();
        }
    }
}