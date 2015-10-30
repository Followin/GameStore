using System;

namespace Pipeline
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
