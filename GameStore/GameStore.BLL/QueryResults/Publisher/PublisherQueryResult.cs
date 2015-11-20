using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.QueryResults.Publisher
{
    public class PublisherQueryResult : IQueryResult
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }

        public string Description { get; set; }

        public string HomePage { get; set; }
    }
}
