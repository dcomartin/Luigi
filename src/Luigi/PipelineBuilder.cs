using System;
using System.Collections.Generic;

namespace Luigi
{
    public interface IPipelineBuilder<TRequest, TResponse>
    {
        void UsePipe<TPipe>();
    }

    public class PipelineBuilder<TRequest, TResponse>: IPipelineBuilder<TRequest, TResponse> 
    {
        private readonly List<Type> _pipes = new List<Type>();
        
        public void UsePipe<TPipe>()
        {
            _pipes.Add(typeof(TPipe));
        }

        public Type[] GetPipes()
        {
            return _pipes.ToArray();
        }
    }
}