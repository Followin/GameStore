using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace GameStore.BLL.CQRS
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private IKernel _kernel;

        public CommandDispatcher(IKernel kernel)
        {
            this._kernel = kernel;
        }

        public void Dispatch<TParameter>(TParameter command) where TParameter : ICommand
        {
            var handler = _kernel.Get<ICommandHandler<TParameter>>();
            handler.Execute(command);
        }
    }
}
