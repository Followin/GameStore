using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using GameStore.BLL.CQRS;
using NLog;

namespace GameStore.Web.ApiControllers
{
    public class BaseApiController : ApiController
    {
        protected BaseApiController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ILogger logger)
        {
            CommandDispatcher = commandDispatcher;
            QueryDispatcher = queryDispatcher;
            Logger = logger;
        }

        protected ICommandDispatcher CommandDispatcher { get; private set; }

        protected IQueryDispatcher QueryDispatcher { get; private set; }

        protected ILogger Logger { get; private set; }

        protected ClaimsPrincipal CurrentPrincipal
        {
            get
            {
                return HttpContext.Current.User as ClaimsPrincipal;
            }
        }
    }
}
