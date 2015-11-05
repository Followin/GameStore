using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;

namespace GameStore.BLL.QueryResults.Order
{
    public class ShippersQueryResult : IEnumerable<ShipperDTO>, IQueryResult
    {
        private IEnumerable<ShipperDTO> _shippers;

        public ShippersQueryResult(IEnumerable<ShipperDTO> shippers)
        {
            _shippers = shippers;
        }

        public IEnumerator<ShipperDTO> GetEnumerator()
        {
            return _shippers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
