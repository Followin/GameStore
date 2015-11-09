using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using GameStore.BLL.Commands.User;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries.User;
using GameStore.BLL.QueryResults.User;
using GameStore.Web.Concrete;
using NLog;

namespace GameStore.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        public AccountController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ILogger logger) : base(commandDispatcher, queryDispatcher, logger)
        {
        }

        
        public ActionResult Login(String sessionId, String returnUrl)
        {
            var user = QueryDispatcher.Dispatch<GetUserBySessionIdQuery, UserQueryResult>(
                new GetUserBySessionIdQuery { SessionId = sessionId });
            if (user == null)
            {
                CommandDispatcher.Dispatch(new CreateUserCommand { SessionId = sessionId });
                user = QueryDispatcher.Dispatch<GetUserBySessionIdQuery, UserQueryResult>(
                new GetUserBySessionIdQuery { SessionId = sessionId });
            }

            var serializeModel = new CustomPrincipalSerializeModel
            {
                Id = user.Id,
                SessionId = sessionId
            };

            var serializer = new JavaScriptSerializer();
            var userData = serializer.Serialize(serializeModel);

            var authTicket = new FormsAuthenticationTicket(
                             version: 1,
                             name: user.Id.ToString(),
                             issueDate: DateTime.UtcNow,
                             expiration: DateTime.UtcNow.AddHours(24),
                             isPersistent: false,
                             userData: userData);
            String encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            Response.Cookies.Add(cookie);

            return RedirectToAction("Index", "Game");
        }

        public ActionResult Ban()
        {
            return View();
        }
    }
}