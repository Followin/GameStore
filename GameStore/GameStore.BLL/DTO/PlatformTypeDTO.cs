using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO
{
    public class PlatformTypeDTO : EntityDTO<Int32>
    {
        public String Name { get; set; }
    }
}
