using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using GameStore.Auth.Abstract;
using GameStore.Auth.Models;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries.Publisher;
using GameStore.BLL.QueryResults.Publisher;
using GameStore.Static;
using GameStore.Web.App_LocalResources;
using GameStore.Web.Filters;
using GameStore.Web.Models.Account;
using GameStore.Web.Models.User;
using NLog;

namespace GameStore.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private IAuthenticationService _auth;
        private IUserService _userService;

        public AccountController(IAuthenticationService auth, ICommandDispatcher commandDispatcher,
            IQueryDispatcher queryDispatcher, IUserService userService, ILogger logger)
            : base(commandDispatcher, queryDispatcher, logger)
        {
            _auth = auth;
            _userService = userService;
        }

        [ClaimsAuthorize(ClaimTypesExtensions.UserPermission, Permissions.Read)]
        public ActionResult Manage(int id)
        {
            var userClaims = _userService.GetUserClaims(id);

            return View(userClaims);
        }

        public JsonResult IsUsernameFree(string name)
        {
            return Json(_userService.IsUsernameFree(name), JsonRequestBehavior.AllowGet);
        }


        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(RegisterAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                _userService.Register(Mapper.Map<RegisterAccountViewModel, RegisterUserModel>(model));
                return RedirectToAction("Index", "Game");
            }
            return View(model);
        }

        [ClaimsAuthorize(ClaimTypesExtensions.UserPermission, Permissions.Add)]
        public ActionResult Create()
        {
            var publishers = QueryDispatcher.Dispatch<GetAllPublishersQuery, PublishersQueryResult>(
                new GetAllPublishersQuery());

            ViewBag.Roles = new[] {Roles.User, Roles.Manager, Roles.Moderator};
            ViewBag.Publishers = new[] {new SelectListItem() {Text = GlobalRes.NotSelected, Value = ""}}
                .Concat(publishers.Select(x => new SelectListItem
                {
                    Text = x.CompanyName,
                    Value = x.Id.ToString()
                }));

            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                _userService.Register(Mapper.Map<CreateUserViewModel, RegisterUserModel>(model));
                return RedirectToAction("Index", "Game");
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl ?? (Request.UrlReferrer != null ? Request.UrlReferrer.AbsolutePath : Url.Action("Index", "Game"))
            };
            return View(model);
        }

        [Authorize]
        public ActionResult Logout()
        {
            _auth.Logout();
            return RedirectToAction("Index", "Game");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model)
        {
            var result = _auth.Login(model.Login, model.Password, model.RememberMe);

            if (result.Status == LoginResultStatus.Success)
            {
                return Redirect(model.ReturnUrl);
            }

            if (result.Status == LoginResultStatus.WrongCredentials)
            {
                ModelState.AddModelError("", GlobalRes.WrongLoginOrPassword);
                return View(model);
            }

            return RedirectToAction("Index", "Game");
        }


        [ClaimsAuthorize(ClaimTypesExtensions.UserPermission, Permissions.Ban)]
        public ActionResult Ban(int userId)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Ban(BanViewModel model)
        {
            if (model.Permanent)
            {
                _userService.BanUser(model.UserId, DateTime.MaxValue);
            }

            var time = DateTime.UtcNow;

            if (model.Hours.HasValue)
            {
                time = time.AddHours(model.Hours.Value);
            }

            if (model.Days.HasValue)
            {
                time = time.AddDays(model.Days.Value);
            }

            if (model.Months.HasValue)
            {
                time = time.AddMonths(model.Months.Value);
            }

            _userService.BanUser(model.UserId, time);

            return RedirectToAction("Index", "Game");
        }
    }
}