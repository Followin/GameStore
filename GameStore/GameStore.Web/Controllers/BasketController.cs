using System;
using System.Security.Claims;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BLL.Commands.Order;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries.Order;
using GameStore.BLL.QueryResults.Order;
using GameStore.Web.Models.Order;
using NLog;

namespace GameStore.Web.Controllers
{
    
    public class BasketController : BaseController
    {
        public BasketController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ILogger logger) : base(commandDispatcher, queryDispatcher, logger)
        {
        }

        [Authorize]
        public ActionResult Index()
        {
            var currentOrder = Mapper.Map<OrderViewModel>(QueryDispatcher.Dispatch<GetCurrentOrder, OrderQueryResult>(
                new GetCurrentOrder
                {
                    UserId = Int32.Parse((User as ClaimsPrincipal).FindFirst(ClaimTypes.SerialNumber).Value)
                }));
            return View(currentOrder);
        }

        [Authorize]
        public ActionResult DeleteDetails(Int32 gameId, Int32 orderId)
        {
            CommandDispatcher.Dispatch(new DeleteOrderDetailsCommand { OrderId = orderId, GameId = gameId });
            return RedirectToAction("Index");
        }

        
    }
}