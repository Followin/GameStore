using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;
using GameStore.BLL.Queries.Game;
using GameStore.BLL.QueryResults.Game;
using GameStore.Web.Models.Genres;
using NLog;

namespace GameStore.Web.ApiControllers
{
    public class GameGenresController : BaseApiController
    {
        public HttpResponseMessage Get(int gameid)
        {
            var game = QueryDispatcher.Dispatch<GetGameByIdQuery, GameQueryResult>(
                new GetGameByIdQuery { Id = gameid });

            if (game == null || game.EntryState != EntryState.Active)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Game not found");
            }

            var displayModel = Mapper.Map<IEnumerable<GenreDTO>, IEnumerable<GenreViewModel>>(game.Genres);

            return Request.CreateResponse(HttpStatusCode.OK, displayModel);
        }

        public GameGenresController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ILogger logger) : base(commandDispatcher, queryDispatcher, logger)
        {
        }
    }
}