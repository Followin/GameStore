using System.Web.Mvc;
using GameStore.BLL.CQRS;
using GameStore.Web.Models.Order;
using NLog;

namespace GameStore.Web.Controllers
{
    
    public class BusketController : BaseController
    {
        public BusketController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ILogger logger) : base(commandDispatcher, queryDispatcher, logger)
        {
        }

        public ActionResult Index(OrderViewModel currentOrder)
        {
            return View(currentOrder);
        }

        
    }
}