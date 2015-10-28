using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.Pipeline
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
