using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.CQRS
{
    public interface ICommandDispatcher
    {
        void Dispatch<TParameter>(TParameter command) where TParameter : ICommand; 
    }
}
