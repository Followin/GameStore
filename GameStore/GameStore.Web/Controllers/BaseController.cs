using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using GameStore.BLL.CQRS;
using Ninject;
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

    }
}