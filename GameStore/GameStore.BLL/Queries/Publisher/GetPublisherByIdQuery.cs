using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Queries.Publisher
{
    public class GetPublisherByIdQuery : IQuery
    {
        public Int32 Id { get; set; }
    }
}
