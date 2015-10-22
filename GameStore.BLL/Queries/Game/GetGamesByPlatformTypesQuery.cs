using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Queries.Game
{
    public class GetGamesByPlatformTypesQuery : IQuery
    {
        public Int32[] Ids { get; set; }

        public String[] Names { get; set; }
    }
}
