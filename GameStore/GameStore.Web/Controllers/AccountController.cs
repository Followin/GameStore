using System;
using System.Web.Mvc;
using AutoMapper;
using GameStore.Auth.Abstract;
using GameStore.Auth.Models;
using GameStore.BLL.CQRS;
using GameStore.Web.App_LocalResources;
using GameStore.Web.Models.Account;
using NLog;

namespace GameStore.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private IAuthenticationService _auth;

        public AccountController(IAuthenticationService auth, ICommandDispatcher commandDispatcher,
            IQueryDispatcher queryDispatcher, ILogger logger)
            : base(commandDispatcher, queryDispatcher, logger)
        {
            _auth = auth;
        }


        

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                _auth.Register(Mapper.Map<RegisterAccountViewModel, RegisterUserModel>(model));
                return RedirectToAction("Index", "Game");
            }
            return View(model);

        }

        public ActionResult Login(String returnUrl)
        {
            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl ?? (Request.UrlReferrer != null ? Request.UrlReferrer.AbsolutePath : Url.Action("Index", "Game"))
            };
            return View(model);
        }

        public ActionResult Logout()
        {
            _auth.Logout();
            return RedirectToAction("Index", "Game");
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            var result = _auth.Login(model.Login, model.Password, model.RememberMe);

            if(result.Status == LoginResultStatus.Success)
            {
                return Redirect(model.ReturnUrl);
            }
            
            if(result.Status == LoginResultStatus.WrongCredentials)
            {
                ModelState.AddModelError("", GlobalRes.WrongLoginOrPassword);
                return View(model);
            }
            
            return RedirectToAction("Index", "Game");
        }

        public ActionResult Ban()
        {
            return View();
        }
    }
}