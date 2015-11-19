﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Metadata.Edm;
using System.Linq;
using System.Linq.Expressions;
using GameStore.DAL.Abstract;
using GameStore.DAL.Abstract.Repositories;
using GameStore.DAL.EF;
using GameStore.DAL.Static;
using GameStore.Domain.Entities;
using GameStore.Static;

namespace GameStore.DAL.Repositories
{
    public class GameRepository : IGameRepository
    {
        private IEFContext _db;
        private INorthwindUnitOfWork _northwind;

        public GameRepository(IEFContext context, INorthwindUnitOfWork northwind)
        {
            _northwind = northwind;
            _db = context;
        }

        public IEnumerable<Game> Get(
            Expression<Func<Game, bool>> predicate,
            GamesOrderType orderBy,
            int? skip = null,
            int? number = null)
        {

            var fullyResult = Get();
                switch (orderBy)
                {
                    case GamesOrderType.Views:
                        fullyResult = fullyResult.OrderBy(x => x.UsersViewed.Count);
                        break;

                    case GamesOrderType.Comments:
                        fullyResult = fullyResult.OrderByDescending(x => _db.Comments.Count(y => y.GameId == x.Id));
                        break;

                    case GamesOrderType.PriceAsc:
                        fullyResult = fullyResult.OrderBy(x => x.Price);
                        break;

                    case GamesOrderType.PriceDesc:
                        fullyResult = fullyResult.OrderByDescending(x => x.Price);
                        break;

                    case GamesOrderType.IncomeDate:
                        fullyResult = fullyResult.OrderByDescending(x => x.IncomeDate);
                        break;
                }
            

            fullyResult = fullyResult.Where(predicate.Compile());

            if (skip.HasValue)
            {
                fullyResult = fullyResult.Skip(skip.Value);
            }

            if (number.HasValue)
            {
                fullyResult = fullyResult.Take(number.Value);
            }

            return fullyResult.ToList();
        }

        private void FillGame(Game game)
        {
            var genresIds =
                _db.GamesGenres.Where(x => x.GameId == game.Id)
                   .Select(x => x.GenreId)
                   .ToList()
                   .GroupBy(KeyEncoder.GetBase)
                   .ToList();
            var mainGenres = genresIds.FirstOrDefault(x => x.Key == DatabaseTypes.GameStore);
            var outGenres = genresIds.FirstOrDefault(x => x.Key == DatabaseTypes.Northwind);

            List<Genre> resultGenres = new List<Genre>();
            if (mainGenres != null)
            {
                resultGenres.AddRange(_db.Genres.Where(x => mainGenres.Contains(x.Id)));
            }
            if (outGenres != null)
            {
                resultGenres.AddRange(outGenres.Select(KeyEncoder.GetId).Select(_northwind.Genres.Get));
            }
            game.Genres = resultGenres;

            if (game.PublisherId.HasValue)
            {
                var publisherDatabase = KeyEncoder.GetBase(game.PublisherId.Value);
                switch (publisherDatabase)
                {
                    case DatabaseTypes.GameStore:
                        game.Publisher = _db.Publishers.Find(game.PublisherId);
                        break;
                    case DatabaseTypes.Northwind:
                        game.Publisher = _northwind.Publishers.Get(KeyEncoder.GetId(game.PublisherId.Value));
                        break;
                }
            }
        }

        public Game GetByKey(string key)
        {
            throw new NotImplementedException();
        }

        public Game Get(int id)
        {
            var database = KeyEncoder.GetBase(id);
            Game game = null;

            switch (database)
            {
                case DatabaseTypes.GameStore:
                    game = _db.Games.Find(id);
                    FillGame(game);
                    break;
                case DatabaseTypes.Northwind:
                    game = _northwind.Games.Get(KeyEncoder.GetId(id));
                    break;
            }

            return game;
        }

        public IEnumerable<Game> Get()
        {
            var games = _db.Games.ToList();
            var exludeIds = games.Where(x => KeyEncoder.GetBase(x.Id) == DatabaseTypes.Northwind).Select(x => KeyEncoder.GetId(x.Id));
            games.ForEach(FillGame);
            games.AddRange(_northwind.Games.GetExluding(exludeIds));
            return games;
        }

        public IEnumerable<Game> Get(Expression<Func<Game, bool>> predicate)
        {
            return Get().Where(predicate.Compile());
        }

        public Game GetFirst(Expression<Func<Game, bool>> predicate)
        {
            return Get().FirstOrDefault(predicate.Compile());
        }

        public int GetCount(Expression<Func<Game, bool>> predicate = null)
        {
            return predicate == null
                ? Get().Count()
                : Get().Count(predicate.Compile());
        }

        public void Add(Game item)
        {
            var nextId = KeyEncoder.GetNext(_db.Games.Select(x => x.Id).Where(x => KeyEncoder.GetBase(x) == DatabaseTypes.GameStore).Max(x => x));
            item.Id = nextId;
            
            foreach (var genre in item.Genres)
            {
                _db.GamesGenres.Add(new GameGenre {GameId = item.Id, GenreId = genre.Id});
            }
            _db.Games.Add(item);
        }

        public void Delete(int id)
        {
            var database = KeyEncoder.GetBase(id);
            switch (database)
            {
                case DatabaseTypes.GameStore:
                    var game = _db.Games.Find(id);
                    game.EntryState = EntryState.Deleted;
                    _db.SetModified(game);
                    break;
                case DatabaseTypes.Northwind:
                    var nGame = _northwind.Games.Get(KeyEncoder.GetId(id));
                    nGame.EntryState = EntryState.Deleted;
                    _db.Games.Add(nGame);
                    break;
            }
        }

        public void Update(Game item)
        {
            var database = KeyEncoder.GetBase(item.Id);
            switch (database)
            {
                case DatabaseTypes.GameStore:
                    _db.SetModified(item);
                    UpdateGameGenres(item);
                    break;
                case DatabaseTypes.Northwind:
                    _db.Games.Add(item);
                    UpdateGameGenres(item);
                    break;
            }
        }

        private void UpdateGameGenres(Game item)
        {
            var existingGenres = _db.GamesGenres.Where(x => x.GameId == item.Id);
            foreach (var genre in existingGenres.Select(x => x.GenreId).Except(item.Genres.Select(x => x.Id)))
            {
                _db.GamesGenres.Remove(_db.GamesGenres.Find(item.Id, genre));
            }
            foreach (var genre in item.Genres.Select(x => x.Id).Except(existingGenres.Select(x => x.GenreId)))
            {
                _db.GamesGenres.Add(new GameGenre {GameId = item.Id, GenreId = genre});
            }
            _db.SaveChanges();
        }
    }

}
