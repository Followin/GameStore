using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Queries.Comment
{
    public class GetCommentByIdQuery : IQuery
    {
        public int Id { get; set; }
    }
}
