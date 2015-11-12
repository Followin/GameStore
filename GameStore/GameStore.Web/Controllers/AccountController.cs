using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using GameStore.Auth.Abstract;
using GameStore.BLL.CQRS;
using GameStore.Web.Concrete;
using NLog;

namespace GameStore.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private IAuthenticationService _auth;
        public AccountController(IAuthenticationService auth, ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ILogger logger) : base(commandDispatcher, queryDispatcher, logger)
        {
            _auth = auth;
        }

        
        public ActionResult Login(String login, String password, String returnUrl)
        {
            _auth.Login(login, password, false);
            return RedirectToAction("Index", "Game");
        }

        public ActionResult Ban()
        {
            return View();
        }
    }
}