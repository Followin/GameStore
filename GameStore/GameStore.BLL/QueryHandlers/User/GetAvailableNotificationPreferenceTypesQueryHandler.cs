using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries.User;
using GameStore.BLL.QueryResults;
using GameStore.DAL.Abstract;
using GameStore.Static;
using NLog;

namespace GameStore.BLL.QueryHandlers.User
{
    public class GetAvailableNotificationPreferenceTypesQueryHandler : 
        IQueryHandler<GetAvailableNotificationPreferenceTypesQuery, StringsQueryResult>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public GetAvailableNotificationPreferenceTypesQueryHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public StringsQueryResult Retrieve(GetAvailableNotificationPreferenceTypesQuery query)
        {
            var result = new List<string>();

            var user = _db.Users.Get(query.UserId);

            if (user.Claims.Any(x => x.Type == ClaimTypes.Email))
            {
                result.Add(NotificationPreferenceTypes.Email);
            }

            if (user.Claims.Any(x => x.Type == ClaimTypes.MobilePhone))
            {
                result.Add(NotificationPreferenceTypes.Sms);
            }

            return new StringsQueryResult(result);
        }
    }
}
