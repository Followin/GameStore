using System;
using System.Security.Claims;
using System.Web.Mvc;
using AutoMapper;
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

        
    }
}