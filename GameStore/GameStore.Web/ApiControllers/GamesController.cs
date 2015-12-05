using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using AutoMapper;
using GameStore.BLL.Commands.Game;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries.Game;
using GameStore.BLL.QueryResults.Game;
using GameStore.Static;
using GameStore.Web.Filters;
using GameStore.Web.Models;
using GameStore.Web.Models.Game;
using NLog;

namespace GameStore.Web.ApiControllers
{
    public class GamesController : BaseApiController
    {
        public HttpResponseMessage Get([FromUri(Name = "filterModel", SuppressPrefixCheck = true)]GameFiltersModel model)
        {
            var query = new GetGamesQuery();

            if (model == null)
            {
                model = new GameFiltersModel();
            }

            if (model.Page == 0)
            {
                model.Page = 1;
            }

            Mapper.Map(model, query);

            var queryResult = QueryDispatcher.Dispatch<GetGamesQuery, GamesPartQueryResult>(query);
            var viewModel = new PagedDisplayGameModel
            {
                DisplayModel = Mapper.Map<GamesPartQueryResult, IEnumerable<DisplayGameModel>>(queryResult),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = model.ItemsPerPage ?? queryResult.Count,
                    CurrentPage = model.Page,
                    TotalItems = queryResult.Count
                }
            };

            return Request.CreateResponse(HttpStatusCode.OK, viewModel);
        }


        public HttpResponseMessage Get(int id)
        {
            var query = new GetGameByIdQuery {Id = id};

            var queryResult = QueryDispatcher.Dispatch<GetGameByIdQuery, GameQueryResult>(query);

            if (queryResult == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Game not found");
            }

            var displayModel = Mapper.Map<GameQueryResult, DisplayGameModel>(queryResult);

            return Request.CreateResponse(HttpStatusCode.OK, displayModel);
        }

        [ClaimsAuthorizeApi(ClaimTypesExtensions.GamePermission, Permissions.Add)]
        public HttpResponseMessage Post([FromBody]CreateGameModel value)
        {
            if (ModelState.IsValid)
            {
                var command = Mapper.Map<CreateGameModel, CreateGameCommand>(value);
                CommandDispatcher.Dispatch(command);

                return new HttpResponseMessage(HttpStatusCode.Created);
            }
            return Request.CreateErrorResponse(HttpStatusCode.Forbidden, ModelState);
        }

        public GamesController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ILogger logger) : base(commandDispatcher, queryDispatcher, logger)
        {
        }
    }
}