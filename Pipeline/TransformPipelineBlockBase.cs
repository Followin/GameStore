namespace Pipeline
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

        /// <summary>
        /// Performs transformation of item and posts it to a next block
        /// </summary>
        /// <param name="item">Item to transform</param>
        /// <returns>Returning item</returns>
        protected abstract TOutput Execute(TInput item);
    }
}
