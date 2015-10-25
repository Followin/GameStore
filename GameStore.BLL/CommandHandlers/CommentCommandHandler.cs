using System;
using System.Linq;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using AutoMapper;
using GameStore.BLL.Commands;
using GameStore.BLL.Commands.Comment;
using GameStore.BLL.CQRS;
using GameStore.BLL.Utils;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using NLog;

namespace GameStore.BLL.CommandHandlers
{
    public class CommentCommandHandler :
    #region interfaces
        ICommandHandler<CreateCommentCommand>,
        ICommandHandler<DeleteCommentCommand>
    #endregion
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public CommentCommandHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            this._db = db;
            this._logger = logger;
        }

        public void Execute(CreateCommentCommand command)
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

            var newComment = Mapper.Map<CreateCommentCommand, Comment>(command);
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
            else
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
        }


        public void Execute(DeleteCommentCommand command)
        {
            command.Id.Argument(NameGetter.GetName(() => command.Id))
                      .GreaterThan(0);

            var comment = _db.Comments.Get(command.Id);

            if (comment == null)
            {
                throw new ArgumentOutOfRangeException(
                    NameGetter.GetName(() => command.Id),
                    "Comment not found");
            }

            if (comment.ChildComments.Any())
            {
                comment.Quotes = null;
                comment.Body = "<Deleted>";
            }
            else
            {
                comment.EntryState = EntryState.Deleted;
            }

            _db.Comments.Update(comment);
            _db.Save();

        }
    }
}
