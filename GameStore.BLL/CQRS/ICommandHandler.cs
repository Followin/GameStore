using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.CQRS
{
    /// <summary>
    /// Handles and executes commands
    /// </summary>
    /// <typeparam name="TParameter">Command type</typeparam>
    public interface ICommandHandler<in TParameter> where TParameter : ICommand
    {
        /// <summary>
        /// Execute command
        /// </summary>
        /// <param name="command">command</param>
        CommandResult Execute(TParameter command);
    }
}
