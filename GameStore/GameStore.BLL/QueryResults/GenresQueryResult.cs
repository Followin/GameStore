using System.Collections;
using System.Collections.Generic;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;

namespace GameStore.BLL.QueryResults
{
    public class GenresQueryResult : IQueryResult, IEnumerable<GenreDTO>
    {
        private IEnumerable<GenreDTO> _genres;

        public GenresQueryResult(IEnumerable<GenreDTO> genres)
        {
            _genres = genres;
        }

        public IEnumerator<GenreDTO> GetEnumerator()
        {
            return _genres.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
