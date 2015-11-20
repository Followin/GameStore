using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using AutoMapper;
using GameStore.BLL.Commands.Order;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;
using GameStore.BLL.Queries.Game;
using GameStore.BLL.Queries.Order;
using GameStore.BLL.QueryResults.Game;
using GameStore.BLL.QueryResults.Order;
using GameStore.Static;
using GameStore.Web.Filters;
using GameStore.Web.Models.Order;
using NLog;

namespace GameStore.Web.ApiControllers
{
    public class OrdersController : BaseApiController
    {
        [ClaimsAuthorizeApi]
        public HttpResponseMessage Get()
        {
            var query = new GetCurrentOrderQuery
            {
                UserId = Convert.ToInt32(CurrentPrincipal.FindFirst(x => x.Type == ClaimTypes.SerialNumber).Value)
            };

            var queryResult = QueryDispatcher.Dispatch<GetCurrentOrderQuery, OrderQueryResult>(query);
            var displayModel = Mapper.Map<OrderQueryResult, OrderViewModel>(queryResult);

            return Request.CreateResponse(HttpStatusCode.OK, displayModel);
        }

        [ClaimsAuthorizeApi]
        public HttpResponseMessage Post(int id)
        {
            var game = QueryDispatcher.Dispatch<GetGameByIdQuery, GameQueryResult>(
                new GetGameByIdQuery { Id = id });

            if (game == null || game.EntryState != EntryState.Active)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Game not found");
            }

            var currentOrder = QueryDispatcher.Dispatch<GetCurrentOrderQuery, OrderQueryResult>(new GetCurrentOrderQuery
            {
                UserId = int.Parse((CurrentPrincipal).FindFirst(ClaimTypes.SerialNumber).Value)
            });
            var newOrderDetails = new CreateOrderDetailsCommand
            {
                Discount = 0F,
                GameId = game.Id,
                OrderId = currentOrder.Id,
                Price = game.Price,
                Quantity = 1
            };

            CommandDispatcher.Dispatch(newOrderDetails);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [ClaimsAuthorizeApi]
        public HttpResponseMessage Delete(int id)
        {
            var game = QueryDispatcher.Dispatch<GetGameByIdQuery, GameQueryResult>(
                new GetGameByIdQuery { Id = id });

            if (game == null || game.EntryState != EntryState.Active)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Game not found");
            }

            var currentOrder = QueryDispatcher.Dispatch<GetCurrentOrderQuery, OrderQueryResult>(new GetCurrentOrderQuery
            {
                UserId = int.Parse((CurrentPrincipal).FindFirst(ClaimTypes.SerialNumber).Value)
            });

            var deleteOrderDetailsCommand = new DeleteOrderDetailsCommand
            {
                GameId = id,
                OrderId = currentOrder.Id
            };

            CommandDispatcher.Dispatch(deleteOrderDetailsCommand);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public OrdersController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ILogger logger) : base(commandDispatcher, queryDispatcher, logger)
        {
        }
    }
}