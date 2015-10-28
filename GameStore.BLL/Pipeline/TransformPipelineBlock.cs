using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.Pipeline
{
    public class TransformPipelineBlock<TInput, TOutput> : TransformPipelineBlockBase<TInput, TOutput>
    {
        private Func<TInput, TOutput> _execute;

        public TransformPipelineBlock(Func<TInput, TOutput> execute)
        {
            _execute = execute;
        }

        protected override TOutput Execute(TInput item)
        {
            return _execute(item);
        }
    }
}
