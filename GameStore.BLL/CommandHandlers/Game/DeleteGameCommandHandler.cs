﻿using System;
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
    class DeleteGameCommandHandler : ICommandHandler<DeleteGameCommand>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public DeleteGameCommandHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
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

    }
}