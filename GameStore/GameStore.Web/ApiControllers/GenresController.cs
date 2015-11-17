using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries.Genre;
using GameStore.BLL.QueryResults.Genre;
using GameStore.Web.Models.Genres;
using NLog;

namespace GameStore.Web.ApiControllers
{
    public class GenresController : BaseApiController
    {
        public HttpResponseMessage Get()
        {
            var query = new GetAllGenresQuery();
            var queryResult = QueryDispatcher.Dispatch<GetAllGenresQuery, GenresQueryResult>(query);
            var model = Mapper.Map<GenresQueryResult, IEnumerable<GenreViewModel>>(queryResult);

            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

        public HttpResponseMessage Get(Int32 id)
        {
            var query = new GetGenreByIdQuery { Id = id };
            var queryResult = QueryDispatcher.Dispatch<GetGenreByIdQuery, GenreQueryResult>(query);

            if (queryResult == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Genre not found");
            }
            
            var model = Mapper.Map<GenreQueryResult, GenreViewModel>(queryResult);

            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

        public GenresController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ILogger logger) : base(commandDispatcher, queryDispatcher, logger)
        {
        }
    }
}