using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO
{
    public class PublisherDTO : EntityDTO<Int32>
    {
        public String CompanyName { get; set; }

        public String Description { get; set; }

        public String HomePage { get; set; }
    }
}
