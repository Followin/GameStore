using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.CQRS
{
    /// <summary>
    /// Dispatches commands
    /// </summary>
    public interface ICommandDispatcher
    {
        /// <summary>
        /// Finds a right command handler and executes it
        /// </summary>
        /// <typeparam name="TParameter">Command type</typeparam>
        /// <param name="command">command</param>
        CommandResult Dispatch<TParameter>(TParameter command) where TParameter : ICommand; 
    }
}
