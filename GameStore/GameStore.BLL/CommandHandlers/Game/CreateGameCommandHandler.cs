using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using AutoMapper;
using GameStore.BLL.Commands.Game;
using GameStore.BLL.CQRS;
using GameStore.BLL.Utils;
using GameStore.BLL.Utils.ValidationExtensions;
using GameStore.Domain.Abstract;
using NLog;

namespace GameStore.BLL.CommandHandlers.Game
{
    public class CreateGameCommandHandler : ICommandHandler<CreateGameCommand>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public CreateGameCommandHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public CommandResult Execute(CreateGameCommand command)
        {
            Validate(command);

            var game = Mapper.Map<Domain.Entities.Game>(command);

            game.Genres = command.GenreIds.Select(g =>
            {
                var genre = _db.Genres.Get(g);
                if (genre == null)
                {
                    throw new EntityNotFoundException(
                        String.Format("Genre not found. Id: {0}", g),
                        NameGetter.GetName(() => command.GenreIds));
                }

                return genre;
            }).ToList();

            game.PlatformTypes = command.PlatformTypeIds.Select(p =>
            {
                var platformType = _db.PlatformTypes.Get(p);
                if (platformType == null)
                {
                    throw new EntityNotFoundException(
                        String.Format("PlatformType not found. Id: {0}", p),
                        NameGetter.GetName(() => command.PlatformTypeIds));
                }

                return platformType;
            }).ToList();

            game.IncomeDate = DateTime.UtcNow;

            _db.Games.Add(game);
            _db.Save();

            return new CommandResult();
        }

        private void Validate(CreateGameCommand command)
        {
            command.Name.Argument(NameGetter.GetName(() => command.Name))
                        .NotNull()
                        .NotWhiteSpace();
            command.Key.Argument(NameGetter.GetName(() => command.Key))
                       .NotNull()
                       .NotWhiteSpace();
            command.DescriptionEn.Argument(NameGetter.GetName(() => command.DescriptionEn))
                               .NotNull()
                               .NotWhiteSpace();
            command.Price.Argument(NameGetter.GetName(() => command.Price))
                         .GreaterThan(0);
            command.UnitsInStock.Argument(NameGetter.GetName(() => command.UnitsInStock))
                                .GreaterThan(-1);

            if (command.PublisherId.HasValue && command.PublisherId < 1)
            {
                throw new ArgumentOutOfRangeException(NameGetter.GetName(() => command.PublisherId));
            }

            if (command.GenreIds != null && !command.GenreIds.All(x => x > 0))
            {
                throw new ArgumentOutOfRangeException(NameGetter.GetName(() => command.GenreIds), "GenreIds must have only greater than zero numbers");
            }

            command.PlatformTypeIds.Argument(NameGetter.GetName(() => command.PlatformTypeIds))
                                   .NotNull()
                                   .NotEmpty()
                                   .AllMatch(
                                        x => x > 0,
                                       "PlatformTypeIds must have only greater than zero numbers");

            if (_db.Games.GetSingle(g => g.Key == command.Key) != null)
            {
                throw new ArgumentException(
                    "There is game with such key in db. Key must be unique.",
                    NameGetter.GetName(() => command.Key));
            }

            if (command.PublisherId.HasValue && _db.Publishers.Get(command.PublisherId.Value) == null)
            {
                throw new ArgumentOutOfRangeException(
                    NameGetter.GetName(() => command.PublisherId),
                    "Publisher not found");
            }
        }
    }
}
