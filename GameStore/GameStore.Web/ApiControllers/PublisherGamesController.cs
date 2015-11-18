using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries.Game;
using GameStore.BLL.QueryResults.Game;
using GameStore.Web.Models.Game;
using NLog;

namespace GameStore.Web.ApiControllers
{
    public class PublisherGamesController : BaseApiController
    {
        public HttpResponseMessage Get(Int32 publisherId)
        {
            var query = new GetGamesByPublisherQuery {Id = publisherId};
            var queryResult = QueryDispatcher.Dispatch<GetGamesByPublisherQuery, GamesQueryResult>(query);

            var displayModel = Mapper.Map<GamesQueryResult, IEnumerable<DisplayGameModel>>(queryResult);

            return Request.CreateResponse(HttpStatusCode.OK, displayModel);
        }

        public PublisherGamesController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ILogger logger) : base(commandDispatcher, queryDispatcher, logger)
        {
        }
    }
}