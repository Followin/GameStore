using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Queries.Publisher
{
    public class GetPublisherByCompanyNameQuery : IQuery
    {
        public string CompanyName { get; set; }
    }
}
