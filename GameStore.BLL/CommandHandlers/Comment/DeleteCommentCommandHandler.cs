using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArgumentValidation;
using ArgumentValidation.Extensions;
using GameStore.BLL.Commands.Comment;
using GameStore.BLL.CQRS;
using GameStore.BLL.Utils;
using GameStore.Domain.Abstract;
using NLog;

namespace GameStore.BLL.CommandHandlers.Comment
{
    public class DeleteCommentCommandHandler : ICommandHandler<DeleteCommentCommand>
    {
        private IGameStoreUnitOfWork _db;
        private ILogger _logger;

        public DeleteCommentCommandHandler(IGameStoreUnitOfWork db, ILogger logger)
        {
            _db = db;
            _logger = logger;
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

            if (comment.ChildComments != null && comment.ChildComments.Any())
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
