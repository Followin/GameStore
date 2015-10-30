namespace Pipeline
{
    public interface ITargetPipelineBlock<in TOutput>
    {
        void Post(TOutput item);
    }

    public interface ISourcePipelineBlock<out TOutput>
    {
        void Register(ITargetPipelineBlock<TOutput> nextItem);
    }

}
