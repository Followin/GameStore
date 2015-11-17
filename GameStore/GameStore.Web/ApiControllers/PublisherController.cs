using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using GameStore.BLL.Commands.Publisher;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries.Genre;
using GameStore.BLL.Queries.Publisher;
using GameStore.BLL.QueryResults.Genre;
using GameStore.BLL.QueryResults.Publisher;
using GameStore.Web.Models.Publisher;
using NLog;

namespace GameStore.Web.ApiControllers
{
    public class PublishersController : BaseApiController
    {
        public HttpResponseMessage Get()
        {
            var query = new GetAllPublishersQuery();
            var queryResult = QueryDispatcher.Dispatch<GetAllPublishersQuery, PublishersQueryResult>(query);
            var model = Mapper.Map<PublishersQueryResult, IEnumerable<DisplayPublisherViewModel>>(queryResult);

            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

        public HttpResponseMessage Get(Int32 id)
        {
            var query = new GetPublisherByIdQuery {Id = id};
            var queryResult = QueryDispatcher.Dispatch<GetPublisherByIdQuery, PublisherQueryResult>(query);

            if (queryResult == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Publisher not found");
            }

            var model = Mapper.Map<PublisherQueryResult, DisplayPublisherViewModel>(queryResult);
            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

        public HttpResponseMessage Post([FromBody] CreatePublisherViewModel model)
        {
            if (ModelState.IsValid)
            {
                var command = Mapper.Map<CreatePublisherViewModel, CreatePublisherCommand>(model);
                CommandDispatcher.Dispatch(command);

                return new HttpResponseMessage(HttpStatusCode.Created);
            }

            return Request.CreateErrorResponse(HttpStatusCode.Forbidden, ModelState);
        }

        public PublishersController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ILogger logger) : base(commandDispatcher, queryDispatcher, logger)
        {
        }
    }
}