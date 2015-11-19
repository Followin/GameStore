using System;
using System.Diagnostics;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using AutoMapper;
using GameStore.BLL.Commands.Comment;
using GameStore.BLL.CQRS;
using GameStore.BLL.Utils;
using GameStore.DAL.Abstract;
using NLog;

namespace GameStore.BLL.CommandHandlers.Comment
{
    public class CreateCommentHandler : ICommandHandler<CreateCommentCommand>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public CreateCommentHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public CommandResult Execute(CreateCommentCommand command)
        {
            command.Argument(NameGetter.GetName(() => command))
                   .NotNull();
            if (command.GameId == null && command.ParentCommentId == null)
            {
                throw new ArgumentNullException(
                    NameGetter.GetName(() => command.GameId) +
                    ", " + NameGetter.GetName(() => command.ParentCommentId),
                    "Either GameId or ParentCommentId must be specified");
            }

            command.Name.Argument("Name")
                        .NotNull()
                        .NotWhiteSpace();
            command.Body.Argument("Body")
                        .NotNull()
                        .NotWhiteSpace();

            var newComment = Mapper.Map<CreateCommentCommand, Domain.Entities.Comment>(command);
            if (command.GameId != null)
            {
                if (command.GameId <= 0)
                {
                    throw new ArgumentOutOfRangeException(
                        NameGetter.GetName(() => command.GameId),
                        "GameId argument must be greater than 0");
                }

                var game = _db.Games.Get(command.GameId.Value);
                if (game == null)
                {
                    throw new ArgumentOutOfRangeException(
                        NameGetter.GetName(
                        () => command.GameId),
                        "Game not found");
                }

                newComment.GameId = game.Id;
            }

            if(command.ParentCommentId.HasValue)
            {
                if (command.ParentCommentId <= 0)
                {
                    throw new ArgumentOutOfRangeException(
                        NameGetter.GetName(() => command.ParentCommentId),
                        "ParentCommentId argument must be greater than 0");
                }

                var comment = _db.Comments.Get(command.ParentCommentId.Value);
                if (comment == null)
                {
                    throw new ArgumentOutOfRangeException(
                        NameGetter.GetName(() => command.ParentCommentId),
                        "Comment not found");
                }

                newComment.ParentCommentId = comment.Id;
            }

            _db.Comments.Add(newComment);
            _db.Save();

            return new CommandResult { Data = newComment.Id };
        }
    }
}
