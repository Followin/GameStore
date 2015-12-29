using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries.User;
using GameStore.BLL.QueryResults;
using GameStore.Static;
using GameStore.Web.Models.Manage;
using NLog;

namespace GameStore.Web.Controllers
{
    public class ManageController : BaseController
    {
        public ActionResult My()
        {
            var currentUserId = int.Parse(CurrentPrincipal.FindFirst(ClaimTypes.SerialNumber).Value);

            var model = new ManageViewModel();
            
            if (CurrentPrincipal.IsInRole(Roles.Manager))
            {
                var availableNotificationPreferenceTypes =
                    QueryDispatcher.Dispatch<GetAvailableNotificationPreferenceTypesQuery, StringsQueryResult>(
                        new GetAvailableNotificationPreferenceTypesQuery {UserId = currentUserId});

                var currentPreference =
                    CurrentPrincipal.HasClaim(x => x.Type == ClaimTypesExtensions.NotificationPreferenceType)
                        ? CurrentPrincipal.FindFirst(x => x.Type == ClaimTypesExtensions.NotificationPreferenceType).Value
                        : string.Empty;

                var preferencesSelectList = availableNotificationPreferenceTypes.Select(x => new SelectListItem
                {
                    Selected = x == currentPreference,
                    Value = x,
                    Text = x
                });

                model.NotificationTypes = preferencesSelectList;
            }

            return View(model);
        }

        public ManageController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ILogger logger) : base(commandDispatcher, queryDispatcher, logger)
        {
        }
    }
}