using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.Utils
{
    public class EntityNotFoundException : ArgumentException
    {
        public EntityNotFoundException(String parameter, String message) : base(parameter, message)
        {
        }
    }
}
