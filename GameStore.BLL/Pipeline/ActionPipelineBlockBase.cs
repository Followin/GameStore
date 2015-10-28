using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.Pipeline
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
