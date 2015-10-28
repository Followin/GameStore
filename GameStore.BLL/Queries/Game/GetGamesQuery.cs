using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.BLL.Queries.Game
{
    public class GetGamesQuery
    {
        public Int32[] GenreIds { get; set; }

        public Int32[] PlatformTypeIds { get; set; }

        public Int32[] PublisherIds { get; set; }

        public Decimal MinPrice { get; set; }

        public Decimal MaxPrice { get; set; }

        public DateTime? MinDate { get; set; }

        public String Name { get; set; }

        public Int32 Items { get; set; }
    }
}
