using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.Pipeline
{
    public abstract class TransformPipelineBlockBase<TInput, TOutput> : ITransformPipelineBlock<TInput, TOutput>
    {
        private ITargetPipelineBlock<TOutput> _next; 

        public void Post(TInput item)
        {
            var result = Execute(item);
            if (_next != null)
            {
                _next.Post(result);
            }
        }

        public void Register(ITargetPipelineBlock<TOutput> nextItem)
        {
            _next = nextItem;
        }

        protected abstract TOutput Execute(TInput item);
    }
}
