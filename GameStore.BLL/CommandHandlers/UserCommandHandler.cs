using System;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using AutoMapper;
using GameStore.BLL.Commands.User;
using GameStore.BLL.CQRS;
using GameStore.BLL.Utils;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using NLog;

namespace GameStore.BLL.CommandHandlers
{
    public class UserCommandHandler : 
    #region interfaces
        ICommandHandler<CreateUserCommand>
    #endregion

    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public UserCommandHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public void Execute(CreateUserCommand command)
        {
            _logger.Info("CreateUserCommand enter");
            Validate(command);

            var user = Mapper.Map<CreateUserCommand, User>(command);
            _db.Users.Add(user);
            _db.Save();
        }

        #region validation

        private void Validate(CreateUserCommand command)
        {
            command.SessionId.Argument(NameGetter.GetName(() => command.SessionId))
                             .NotNull()
                             .NotWhiteSpace();
            if(_db.Users.GetSingle(x => x.SessionId == command.SessionId) != null)
                throw new ArgumentException(
                    "User with such SessionId already exists",
                    NameGetter.GetName(() => command.SessionId));
        }
        #endregion
    }
}
