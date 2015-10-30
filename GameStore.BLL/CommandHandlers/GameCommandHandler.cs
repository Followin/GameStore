using System;
using System.Linq;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using AutoMapper;
using GameStore.BLL.Commands;
using GameStore.BLL.Commands.Game;
using GameStore.BLL.CQRS;
using GameStore.BLL.Utils;
using GameStore.BLL.Utils.ValidationExtensions;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using NLog;

namespace GameStore.BLL.CommandHandlers
{
    public class GameCommandHandler :
    #region interfaces
 ICommandHandler<CreateGameCommand>,
        ICommandHandler<DeleteGameCommand>,
        ICommandHandler<EditGameCommand>,
        ICommandHandler<AddGameVisitCommand>
    #endregion
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public GameCommandHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public void Execute(CreateGameCommand command)
        {
            Validate(command);

            var game = Mapper.Map<Game>(command);

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
        }

        public void Execute(DeleteGameCommand command)
        {
            command.Argument(NameGetter.GetName(() => command))
                   .NotNull();
            command.Key.Argument(NameGetter.GetName(() => command.Key))
                   .NotNull()
                   .NotWhiteSpace();

            var game = _db.Games.GetSingle(g => g.Key == command.Key);

            if (game == null)
            {
                throw new ArgumentOutOfRangeException(
                    NameGetter.GetName(() => command.Key),
                    "Game not found");
            }

            game.EntryState = EntryState.Deleted;
            _db.Games.Update(game);
            _db.Save();
        }

        public void Execute(EditGameCommand command)
        {
            var game = Validate(command);

            Mapper.Map(command, game);
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

            _db.Games.Update(game);
            _db.Save();
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

        #region Validators
        private void Validate(CreateGameCommand command)
        {
            command.Name.Argument(NameGetter.GetName(() => command.Name))
                        .NotNull()
                        .NotWhiteSpace();

            command.Key.Argument(NameGetter.GetName(() => command.Key))
                       .NotNull()
                       .NotWhiteSpace();
            command.Description.Argument(NameGetter.GetName(() => command.Description))
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
            if (!command.GenreIds.All(x => x > 0))
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

            if (_db.Publishers.Get(command.PublisherId) == null)
            {
                throw new ArgumentOutOfRangeException(
                    NameGetter.GetName(() => command.PublisherId),
                    "Publisher not found");
            }
        }

        private Game Validate(EditGameCommand command)
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
            command.Description.Argument(NameGetter.GetName(() => command.Description))
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
            if (!command.GenreIds.All(x => x > 0))
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

            if (command.Key != game.Key && _db.Games.GetSingle(g => g.Key == command.Key) != null)
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

        private void Validate(AddGameVisitCommand command)
        {
            command.GameId.Argument(NameGetter.GetName(() => command.GameId))
                          .GreaterThan(0);
            command.UserId.Argument(NameGetter.GetName(() => command.UserId))
                          .GreaterThan(0);
        }
        #endregion


    }
}
