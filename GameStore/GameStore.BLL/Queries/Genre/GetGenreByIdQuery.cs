using System;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.Queries.Genre
{
    public class GetGenreByIdQuery : IQuery
    {
        public int Id { get; set; }
    }
}
