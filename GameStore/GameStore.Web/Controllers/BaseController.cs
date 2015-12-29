using System.Collections.ObjectModel;
using System.Security.Claims;
using System.Web.Mvc;
using GameStore.BLL.CQRS;
using GameStore.Web.Models;
using NLog;

namespace GameStore.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected BaseController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ILogger logger)
        {
            CommandDispatcher = commandDispatcher;
            QueryDispatcher = queryDispatcher;
            Logger = logger;
        }

        protected ICommandDispatcher CommandDispatcher { get; private set; }

        protected IQueryDispatcher QueryDispatcher { get; private set; }

        protected ILogger Logger { get; private set; }

        protected void TempMessage(TempMessageType type, string message, string linkText = null, string linkHref = null)
        {
            TempMessage tempMessage;
            if (linkText != null && linkHref != null)
                tempMessage = new LinkTempMessage(type, message, linkText, linkHref);
            else tempMessage = new TempMessage(type, message);

            if (TempData.ContainsKey("TempMessages"))
                ((Collection<TempMessage>)(TempData["TempMessages"])).Add(tempMessage);
            else TempData.Add("TempMessages", new Collection<TempMessage> { tempMessage });
        }

        protected void SuccessMessage(string message, string linkText = null, string linkHref = null)
        {
            TempMessage(TempMessageType.Success, message, linkText, linkHref);
        }

        protected void ErrorMessage(string message, string linkText = null, string linkHref = null)
        {
            TempMessage(TempMessageType.Error, message, linkText, linkHref);
        }

        protected ClaimsPrincipal CurrentPrincipal
        {
            get { return HttpContext.User as ClaimsPrincipal; }
        }

    }
}