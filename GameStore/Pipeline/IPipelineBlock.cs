namespace Pipeline
{
    public interface ITargetPipelineBlock<in TOutput>
    {
        /// <summary>
        /// Post item through pipeline
        /// </summary>
        /// <param name="item"></param>
        void Post(TOutput item);
    }

    public interface ISourcePipelineBlock<out TOutput>
    {
        /// <summary>
        /// Register next pipeline block
        /// </summary>
        /// <param name="nextItem"></param>
        void Register(ITargetPipelineBlock<TOutput> nextItem);
    }

}
