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
            _kernel = kernel;
        }

        public CommandResult Dispatch<TParameter>(TParameter command) where TParameter : ICommand
        {
            var handler = _kernel.Get<ICommandHandler<TParameter>>();
            return handler.Execute(command);
        }
    }
}
