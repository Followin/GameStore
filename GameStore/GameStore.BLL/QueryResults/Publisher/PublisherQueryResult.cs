using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.QueryResults.Publisher
{
    public class PublisherQueryResult : IQueryResult
    {
        public Int32 Id { get; set; }

        public String CompanyName { get; set; }

        public String Description { get; set; }

        public String HomePage { get; set; }
    }
}
