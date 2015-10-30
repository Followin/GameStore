namespace Pipeline
{
    public abstract class ActionPipelineBlockBase<TInput> : ITargetPipelineBlock<TInput>
    {
        public void Post(TInput item)
        {
            Execute(item);
        }

        protected abstract void Execute(TInput item);
    }
}
