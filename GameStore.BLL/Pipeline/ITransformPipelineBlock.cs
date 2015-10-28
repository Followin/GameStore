namespace GameStore.BLL.Pipeline
{
    public interface ITransformPipelineBlock<in TInput, out TOutput> :
        ITargetPipelineBlock<TInput>,
        ISourcePipelineBlock<TOutput>
    {
    }
}