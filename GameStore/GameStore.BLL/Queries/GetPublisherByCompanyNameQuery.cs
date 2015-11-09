using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Queries
{
    public class GetPublisherByCompanyNameQuery : IQuery
    {
        public String CompanyName { get; set; }
    }
}
