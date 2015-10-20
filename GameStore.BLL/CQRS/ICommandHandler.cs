using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.CQRS
{
    public interface ICommandHandler<in TParameter> where TParameter : ICommand
    {
        void Execute(TParameter command);
    }
}
