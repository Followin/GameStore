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
using GameStore.DAL.Abstract;
using NLog;

namespace GameStore.BLL.CommandHandlers.Game
{
    public class EditGameCommandHandler : ICommandHandler<EditGameCommand>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public EditGameCommandHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public CommandResult Execute(EditGameCommand command)
        {
            var game = Validate(command);

            Mapper.Map(command, game);
            game.Genres = command.GenreIds.Select(g =>
            {
                var genre = _db.Genres.Get(g);
                if (genre == null)
                {
                    throw new EntityNotFoundException(
                        string.Format("Genre not found. Id: {0}", g),
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
                        string.Format("PlatformType not found. Id: {0}", p),
                        NameGetter.GetName(() => command.PlatformTypeIds));
                }

                return platformType;
            }).ToList();

            _db.Games.Update(game);
            _db.Save();
            
            return new CommandResult();
        }

        private Domain.Entities.Game Validate(EditGameCommand command)
        {
            command.Argument(NameGetter.GetName(() => command))
                   .NotNull();
            command.Id.Argument(NameGetter.GetName(() => command.Id))
                      .GreaterThan(0);
            command.Name.Argument(NameGetter.GetName(() => command.Name))
                        .NotNull()
                        .NotWhiteSpace()
                        .ShorterThan(51);
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
            command.PublisherId.Argument(NameGetter.GetName(() => command.PublisherId))
                               .GreaterThan(0);
            command.GenreIds.Argument(NameGetter.GetName(() => command.GenreIds))
                            .NotNull()
                            .NotEmpty();
            if (command.GenreIds != null && !command.GenreIds.All(x => x > 0))
            {
                throw new ArgumentOutOfRangeException(NameGetter.GetName(() => command.GenreIds), "GenreIds must have only greater than zero numbers");
            }

            command.PlatformTypeIds.Argument("PlatformTypeIds")
                                   .NotNull()
                                   .NotEmpty()
                                   .AllMatch(
                                        x => x > 0,
                                        "PlatformTypeIds must have only greater than zero numbers");

            var game = _db.Games.Get(command.Id);
            if (game == null)
            {
                throw new ArgumentOutOfRangeException(NameGetter.GetName(() => command.Id), "Game not found");
            }

            if (command.Key != game.Key && _db.Games.GetFirst(g => g.Key == command.Key) != null)
            {
                throw new ArgumentException(
                    "Game with such key already exists",
                    NameGetter.GetName(() => command.Key));
            }

            if (_db.Publishers.Get(command.PublisherId) == null)
            {
                throw new ArgumentOutOfRangeException(
                    NameGetter.GetName(() => command.PublisherId),
                    "Publisher not found");
            }

            return game;
        }
    }
}
