using System;
using System.Linq;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using AutoMapper;
using GameStore.BLL.Commands;
using GameStore.BLL.CQRS;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using NLog;

namespace GameStore.BLL.CommandHandlers
{
    public class GameCommandHandler :
        ICommandHandler<CreateGameCommand>,
        ICommandHandler<DeleteGameCommand>,
        ICommandHandler<EditGameCommand>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public GameCommandHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            this._db = db;
            this._logger = logger;
        }

        public void Execute(CreateGameCommand command)
        {
            command.Name.Argument("Name")
                        .NotNull()
                        .NotWhiteSpace();
            command.Key.Argument("Key")
                       .NotNull()
                       .NotWhiteSpace();
            command.Description.Argument("Description")
                               .NotNull()
                               .NotWhiteSpace();
            command.GenreIds.Argument("GenreIds")
                            .NotNull()
                            .NotEmpty();
            if (!command.GenreIds.All(x => x > 0))
            {
                throw new ArgumentOutOfRangeException("Ids", "GenreIds must have only greater than zero numbers");
            }

            command.PlatformTypeIds.Argument("PlatformTypeIds")
                                   .NotNull()
                                   .NotEmpty()
                                   .AllMatch(
                                        x => x > 0,
                                       "PlatformTypeIds must have only greater than zero numbers");

            if (_db.Games.GetSingle(g => g.Key == command.Key) != null)
            {
                throw new ArgumentException("There is game with such key in db. Key must be unique.", "Key");
            }

            var game = Mapper.Map<Game>(command);

            game.Genres = command.GenreIds.Select(g =>
            {
                var genre = _db.Genres.Get(g);
                if (genre == null)
                {
                    throw new ArgumentException(
                        String.Format("Genre not found. Id: {0}", g),
                                      "GenreIds");
                }

                return genre;
            }).ToList();

            game.PlatformTypes = command.PlatformTypeIds.Select(p =>
            {
                var platformType = _db.PlatformTypes.Get(p);
                if (platformType == null)
                {
                    throw new ArgumentException(
                        String.Format("PlatformType not found. Id: {0}", p),
                                      "PlatformTypeIds");
                }

                return platformType;
            }).ToList();

            _db.Games.Add(game);
            _db.Save();
        }

        public void Execute(DeleteGameCommand command)
        {
            command.Argument("command")
                   .NotNull();
            command.Key.Argument("Key")
                   .NotNull()
                   .NotWhiteSpace();

            var game = _db.Games.GetSingle(g => g.Key == command.Key);

            if (game == null)
            {
                throw new ArgumentOutOfRangeException("Key", "Game not found");
            }

            game.EntryState = EntryState.Deleted;
            _db.Games.Update(game);
            _db.Save();
        }

        public void Execute(EditGameCommand command)
        {
            command.Argument("command")
                   .NotNull();
            command.Id.Argument("Id")
                      .GreaterThan(0);
            command.Name.Argument("Name")
                        .NotNull()
                        .NotWhiteSpace()
                        .ShorterThan(51);
            command.Key.Argument("Key")
                       .NotNull()
                       .NotWhiteSpace();
            command.Description.Argument("Description")
                               .NotNull()
                               .NotWhiteSpace();
            command.GenreIds.Argument("GenreIds")
                            .NotNull()
                            .NotEmpty();
            if (!command.GenreIds.All(x => x > 0))
            {
                throw new ArgumentOutOfRangeException("Ids", "GenreIds must have only greater than zero numbers");
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
                throw new ArgumentOutOfRangeException("Id", "Game not found");
            }

            if (command.Key != game.Key && _db.Games.GetSingle(g => g.Key == command.Key) != null)
            {
                throw new ArgumentException("Game with such key already exists", "Key");
            }

            Mapper.Map(command, game);
            game.Genres = command.GenreIds.Select(g =>
            {
                var genre = _db.Genres.Get(g);
                if (genre == null)
                {
                    throw new ArgumentException(
                        String.Format("Genre not found. Id: {0}", g),
                                      "GenreIds");
                }

                return genre;
            }).ToList();

            game.PlatformTypes = command.PlatformTypeIds.Select(p =>
            {
                var platformType = _db.PlatformTypes.Get(p);
                if (platformType == null)
                {
                    throw new ArgumentException(
                        String.Format("PlatformType not found. Id: {0}", p),
                                      "PlatformTypeIds");
                }

                return platformType;
            }).ToList();

            _db.Games.Update(game);
            _db.Save();
        }
    }
}
