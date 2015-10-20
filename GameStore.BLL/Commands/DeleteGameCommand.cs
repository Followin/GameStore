using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries;

namespace GameStore.BLL.Commands
{
    public class DeleteGameCommand : ICommand
    {
        public String Key { get; set; }
    }
}
