using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO
{
    public class CommentDTO : EntityDTO<Int32>
    {
        public String Name { get; set; }

        public String Quotes { get; set; }

        public String Body { get; set; }

        public IEnumerable<CommentDTO> ChildComments { get; set; } 
    }
}
