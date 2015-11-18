using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using GameStore.BLL.Commands.Comment;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries.Comment;
using GameStore.BLL.QueryResults.Comment;
using GameStore.Static;
using GameStore.Web.Filters;
using GameStore.Web.Models.Comment;
using NLog;

namespace GameStore.Web.ApiControllers
{
    public class CommentsController : BaseApiController
    {
        public HttpResponseMessage Get(Int32 gameId)
        {
            var query = new GetCommentsForGameQuery {Id = gameId};
            var queryResult = QueryDispatcher.Dispatch<GetCommentsForGameQuery, CommentsQueryResult>(query);
            var model = Mapper.Map<CommentsQueryResult, IEnumerable<DisplayCommentViewModel>>(queryResult);

            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

        public HttpResponseMessage Get(Int32 gameId, Int32 id)
        {
            var query = new GetCommentByIdQuery {Id = id};
            var queryResult = QueryDispatcher.Dispatch<GetCommentByIdQuery, CommentQueryResult>(query);

            if (queryResult == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Comment not found");
            }

            var model = Mapper.Map<CommentQueryResult, DisplayCommentViewModel>(queryResult);

            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

        [ClaimsAuthorizeApi(ClaimTypesExtensions.CommentPermission, Permissions.Delete)]
        public HttpResponseMessage Delete(Int32 gameId, Int32 id)
        {
            var deleteCommentCommand = new DeleteCommentCommand { Id = id };

            var commandResult = CommandDispatcher.Dispatch(deleteCommentCommand);

            return !commandResult.Success 
                ? new HttpResponseMessage(HttpStatusCode.Forbidden) 
                : new HttpResponseMessage(HttpStatusCode.OK);
        }

        [ClaimsAuthorizeApi(ClaimTypesExtensions.CommentPermission, Permissions.Add)]
        public HttpResponseMessage Post(Int32 gameId, [FromBody] CreateCommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.GameId = gameId;

                var command = Mapper.Map<CreateCommentViewModel, CreateCommentCommand>(model);
                CommandDispatcher.Dispatch(command);

                return new HttpResponseMessage(HttpStatusCode.Created);
            }
            return Request.CreateErrorResponse(HttpStatusCode.Forbidden, ModelState);
        }

        public CommentsController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ILogger logger) : base(commandDispatcher, queryDispatcher, logger)
        {
        }
    }
}