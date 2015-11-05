using System;
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

        public ActionResult Index()
        {
            var currentOrder = Mapper.Map<OrderViewModel>(QueryDispatcher.Dispatch<GetCurrentOrder, OrderQueryResult>(
                new GetCurrentOrder {UserId = 1}));
            return View(currentOrder);
        }

        
    }
}