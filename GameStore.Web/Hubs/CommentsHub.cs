using System;
using System.Threading.Tasks;
using GameStore.BLL.CommandHandlers.Comment;
using GameStore.BLL.Commands.Comment;
using GameStore.BLL.CQRS;
using GameStore.Web.Models.Comment;
using Microsoft.AspNet.SignalR;
using Ninject;

namespace GameStore.Web.Hubs
{
    public class CommentsHub : Hub
    {
        private ICommandDispatcher _commandDispatcher;

        public CommentsHub(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task JoinGroup(String groupName)
        {
            return Groups.Add(Context.ConnectionId, groupName);
        }

        public Task LeaveGroup(String groupName)
        {
            return Groups.Remove(Context.ConnectionId, groupName);
        }

        public void CreateComment(String gameId, String parentId, String name, String quotes, String body)
        {
            var createCommentCommand = new CreateCommentCommand
            {
                GameId = Int32.Parse(gameId),
                Body = body,
                Name = name,
                Quotes = quotes,
            };

            if (!String.IsNullOrWhiteSpace(parentId))
            {
                createCommentCommand.ParentCommentId = Int32.Parse(parentId);
            }

            var commandResult = _commandDispatcher.Dispatch(createCommentCommand);
            var id = (Int32)commandResult.Data;

            Clients.Group(gameId).addComment(id, parentId, name, quotes, body);
        }

        public void DeleteComment(String gameId, String commentId)
        {
            var deleteCommentCommand = new DeleteCommentCommand { Id = Int32.Parse(commentId) };

            var commandResult = _commandDispatcher.Dispatch(deleteCommentCommand);
            if (commandResult.Success)
            {
                Clients.Group(gameId).deleteComment(commentId);
            }
            else
            {
                Clients.Group(gameId).changeCommentBody(commentId, commandResult.Data as String);
            }
        }




    }
}