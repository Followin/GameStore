using System;

namespace Pipeline
{
    public class ActionPipelineBlock<TInput> : ActionPipelineBlockBase<TInput>
    {
        private Action<TInput> _action;

        /// <summary>
        /// Creates new block ready to get chained
        /// </summary>
        /// <param name="action">Action to perform on item</param>
        public ActionPipelineBlock(Action<TInput> action)
        {
            _action = action;
        }

        protected override void Execute(TInput item)
        {
            _action(item);
        }
    }
}
