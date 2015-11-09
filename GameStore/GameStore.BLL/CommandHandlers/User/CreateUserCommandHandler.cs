using System;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using AutoMapper;
using GameStore.BLL.Commands.User;
using GameStore.BLL.CQRS;
using GameStore.BLL.Utils;
using GameStore.Domain.Abstract;
using NLog;

namespace GameStore.BLL.CommandHandlers.User
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public CreateUserCommandHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public CommandResult Execute(CreateUserCommand command)
        {
            _logger.Info("CreateUserCommand enter");
            Validate(command);

            var user = Mapper.Map<CreateUserCommand, Domain.Entities.User>(command);

            _db.Users.Add(user);
            _db.Save();

            return new CommandResult();
        }

        #region validation

        private void Validate(CreateUserCommand command)
        {
            command.SessionId.Argument(NameGetter.GetName(() => command.SessionId))
                             .NotNull()
                             .NotWhiteSpace();
            if (_db.Users.GetSingle(x => x.SessionId == command.SessionId) != null)
                throw new ArgumentException(
                    "User with such SessionId already exists",
                    NameGetter.GetName(() => command.SessionId));
        }
        #endregion
    }
}
