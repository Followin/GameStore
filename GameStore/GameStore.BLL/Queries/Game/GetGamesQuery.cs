using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.CQRS;
using GameStore.Static;

namespace GameStore.BLL.Queries.Game
{
    public class GetGamesQuery : IQuery
    {
        public int[] GenreIds { get; set; }

        public int[] PlatformTypeIds { get; set; }

        public int[] PublisherIds { get; set; }

        public decimal MinPrice { get; set; }

        public decimal MaxPrice { get; set; }

        public DateTime? MinDate { get; set; }

        public string Name { get; set; }

        public int? Number { get; set; }

        public int? Skip { get; set; }

        public GamesOrderType OrderBy { get; set; } 
    }
}
