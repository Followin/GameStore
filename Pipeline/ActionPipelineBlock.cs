using System;

namespace Pipeline
{
    public class ActionPipelineBlock<TInput> : ActionPipelineBlockBase<TInput>
    {
        private Action<TInput> _action;

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
