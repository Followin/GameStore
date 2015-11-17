using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using GameStore.BLL.CQRS;
using NLog;

namespace GameStore.Web.ApiControllers
{
    public class AccountController : BaseApiController
    {
        public IEnumerable<Claim> Get()
        {
            return CurrentPrincipal.Claims.ToList();
        }

        public AccountController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ILogger logger) : base(commandDispatcher, queryDispatcher, logger)
        {
        }
    }
}