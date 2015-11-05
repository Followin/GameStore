using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using GameStore.BLL.Commands.Game;
using GameStore.BLL.CQRS;
using GameStore.BLL.Utils;
using GameStore.Domain.Abstract;
using NLog;

namespace GameStore.BLL.CommandHandlers.Game
{
    public class AddVisitCommandHandler : ICommandHandler<AddGameVisitCommand>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public AddVisitCommandHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public void Execute(AddGameVisitCommand command)
        {
            Validate(command);

            var game = _db.Games.Get(command.GameId);
            if (game == null)
                throw new ArgumentOutOfRangeException(
                    NameGetter.GetName(() => command.GameId),
                    "Game not found");

            var user = _db.Users.Get(command.UserId);
            if (user != null)
            {
                //throw new ArgumentOutOfRangeException(
                //    NameGetter.GetName(() => command.UserId),
                //    "User not found");


                game.UsersViewed.Add(user);

                _db.Games.Update(game);
                _db.Save();
            }
        }

        private void Validate(AddGameVisitCommand command)
        {
            command.GameId.Argument(NameGetter.GetName(() => command.GameId))
                          .GreaterThan(0);
            command.UserId.Argument(NameGetter.GetName(() => command.UserId))
                          .GreaterThan(0);
        }
    }
}
