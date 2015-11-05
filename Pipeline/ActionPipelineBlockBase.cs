namespace Pipeline
{
    public abstract class ActionPipelineBlockBase<TInput> : ITargetPipelineBlock<TInput>
    {
        public void Post(TInput item)
        {
            Execute(item);
        }

        /// <summary>
        /// Action to execute on recieving an item
        /// </summary>
        /// <param name="item">Recieved item</param>
        protected abstract void Execute(TInput item);
    }
}
