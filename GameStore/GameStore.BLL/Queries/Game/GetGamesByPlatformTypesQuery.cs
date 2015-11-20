using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Queries.Game
{
    public class GetGamesByPlatformTypesQuery : IQuery
    {
        public int[] Ids { get; set; }

        public string[] Names { get; set; }
    }
}
