using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.CQRS
{
    public class CommandResult
    {
        public CommandResult()
        {
            Success = true;
        }

        public bool Success { get; set; }

        public object Data { get; set; }
    }
}
