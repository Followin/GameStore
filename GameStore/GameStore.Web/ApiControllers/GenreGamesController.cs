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
    public class GenreGamesController : BaseApiController
    {
        public HttpResponseMessage Get(int genreId)
        {
            var query = new GetGamesByGenreQuery {Id = genreId};
            var queryResult = QueryDispatcher.Dispatch<GetGamesByGenreQuery, GamesQueryResult>(query);

            var displayModel = Mapper.Map<GamesQueryResult, IEnumerable<DisplayGameModel>>(queryResult);

            return Request.CreateResponse(HttpStatusCode.OK, displayModel);
        }

        public GenreGamesController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ILogger logger) : base(commandDispatcher, queryDispatcher, logger)
        {
        }
    }
}