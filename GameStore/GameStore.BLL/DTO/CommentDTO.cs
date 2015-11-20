using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.DTO
{
    public class CommentDTO : EntityDTO<int>
    {
        public string Name { get; set; }

        public string Quotes { get; set; }

        public string Body { get; set; }

        public IEnumerable<CommentDTO> ChildComments { get; set; } 
    }
}
