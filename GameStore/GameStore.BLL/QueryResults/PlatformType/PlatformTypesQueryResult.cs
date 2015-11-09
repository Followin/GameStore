using System.Collections;
using System.Collections.Generic;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;

namespace GameStore.BLL.QueryResults.PlatformType
{
    public class PlatformTypesQueryResult : IQueryResult, IEnumerable<PlatformTypeDTO>
    {
        private IEnumerable<PlatformTypeDTO> _platformTypes;

        public PlatformTypesQueryResult(IEnumerable<PlatformTypeDTO> platformTypes)
        {
            _platformTypes = platformTypes;
        }

        public IEnumerator<PlatformTypeDTO> GetEnumerator()
        {
            return _platformTypes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
