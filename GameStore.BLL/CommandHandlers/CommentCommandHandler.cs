using System;
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
    public class CommentCommandHandler :
        ICommandHandler<CreateCommentCommand>
    {
        private IGameStoreUnitOfWork db;
        private ILogger logger;

        public CommentCommandHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            this.db = db;
            this.logger = logger;
        }


        public void Execute(CreateCommentCommand command)
        {
            command.Argument("command")
                   .NotNull();
            if(command.GameId == null && command.ParentCommentId == null)
                throw new ArgumentNullException("GameId, ParentCommentId",
                    "Either GameId or ParentCommentId must be specified");
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
                    throw new ArgumentOutOfRangeException("GameId",
                        "GameId argument must be greater than 0");

                var game = db.Games.Get(command.GameId.Value);
                if (game == null)
                    throw new ArgumentOutOfRangeException("GameId", "Game not found");

                newComment.GameId = game.Id;
            }
            else
            {
                if(command.ParentCommentId <= 0)
                    throw new ArgumentOutOfRangeException("ParentCommentId",
                        "ParentCommentId argument must be greater than 0");

                var comment = db.Comments.Get(command.ParentCommentId.Value);
                if(comment == null)
                    throw new ArgumentOutOfRangeException("ParentCommentId", "Comment not found");

                newComment.ParentCommentId = comment.Id;
            }

            db.Comments.Add(newComment);
            db.Save();
        }
    }
}
