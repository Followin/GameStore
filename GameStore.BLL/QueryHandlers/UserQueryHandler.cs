using ArgumentValidation;
using ArgumentValidation.Extensions;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries.User;
using GameStore.BLL.QueryResults.User;
using GameStore.BLL.Utils;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using NLog;

namespace GameStore.BLL.QueryHandlers
{
    public class UserQueryHandler : 
    #region interfaces
        IQueryHandler<GetUserBySessionIdQuery,UserQueryResult>
    #endregion

    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public UserQueryHandler(IGameStoreUnitOfWork db, ILogger logger)
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
                user = new User() {Id = 1, SessionId = "Hello comprendo"};
            }
            return Mapper.Map<User, UserQueryResult>(user);
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
