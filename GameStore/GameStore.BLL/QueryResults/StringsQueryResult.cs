using System.Collections;
using System.Collections.Generic;
using GameStore.BLL.CQRS;

namespace GameStore.BLL.QueryResults
{
    public class StringsQueryResult : IEnumerable<string>, IQueryResult
    {
        private readonly IEnumerable<string> _strings;

        public StringsQueryResult(IEnumerable<string> strings)
        {
            _strings = strings;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _strings.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
