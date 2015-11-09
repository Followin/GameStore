using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;

namespace GameStore.BLL.QueryResults
{
    public class PublishersQueryResult : IQueryResult, IEnumerable<PublisherDTO>
    {
        private IEnumerable<PublisherDTO> _publishers;

        public PublishersQueryResult(IEnumerable<PublisherDTO> publishers)
        {
            _publishers = publishers;
        }

        public IEnumerator<PublisherDTO> GetEnumerator()
        {
            return _publishers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
