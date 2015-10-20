using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.BLL.CQRS;
using Ninject;
using NLog;

namespace GameStore.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected ICommandDispatcher CommandDispatcher;
        protected IQueryDispatcher QueryDispatcher;
        protected ILogger Logger;

        protected BaseController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ILogger logger)
        {
            CommandDispatcher = commandDispatcher;
            QueryDispatcher = queryDispatcher;
            Logger = logger;
        }
    }
}