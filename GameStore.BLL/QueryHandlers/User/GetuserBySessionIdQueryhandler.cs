using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries.User;
using GameStore.BLL.QueryResults.User;
using GameStore.BLL.Utils;
using GameStore.Domain.Abstract;
using NLog;

namespace GameStore.BLL.QueryHandlers.User
{
    public class GetuserBySessionIdQueryhandler : IQueryHandler<GetUserBySessionIdQuery, UserQueryResult>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public GetuserBySessionIdQueryhandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public UserQueryResult Retrieve(GetUserBySessionIdQuery query)
        {
            Validate(query);

            var user = _db.Users.GetSingle(x => x.SessionId == query.SessionId);
            if (user == null)
            {
                user = new Domain.Entities.User() { Id = 1, SessionId = "Hello comprendo" };
            }
            return Mapper.Map<Domain.Entities.User, UserQueryResult>(user);
        }

        #region Validation

        private void Validate(GetUserBySessionIdQuery query)
        {
            query.SessionId.Argument(NameGetter.GetName(() => query.SessionId))
                           .NotNull()
                           .NotWhiteSpace();
        }
        #endregion
    }
}
