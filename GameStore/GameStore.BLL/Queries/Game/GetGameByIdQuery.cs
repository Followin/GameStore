using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Queries.Game
{
    public class GetGameByIdQuery : IQuery
    {
        public int Id { get; set; }
    }
}
