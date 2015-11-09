using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Queries.Game
{
    public class GetGameByKeyQuery : IQuery
    {
        public String Key { get; set; }
    }
}
