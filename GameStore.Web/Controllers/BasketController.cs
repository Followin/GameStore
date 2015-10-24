using System;
using System.Web.Mvc;
using GameStore.BLL.CQRS;
using GameStore.Web.Models.Order;
using NLog;

namespace GameStore.Web.Controllers
{
    
    public class BasketController : BaseController
    {
        public BasketController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ILogger logger) : base(commandDispatcher, queryDispatcher, logger)
        {
        }

        public ActionResult Index(OrderViewModel currentOrder, String sessionId)
        {
            return View(currentOrder);
        }

        
    }
}