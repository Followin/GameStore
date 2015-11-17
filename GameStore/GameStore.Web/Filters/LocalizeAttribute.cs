using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Http.Filters;
using MvcActionFilterAttribute = System.Web.Mvc.ActionFilterAttribute;
using HttpActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;

namespace GameStore.Web.Filters
{
    public class LocalizeAttribute : MvcActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var cultureName = filterContext.RouteData.Values.ContainsKey("lang") ? filterContext.RouteData.Values["lang"].ToString() : "en";

            List<string> cultures = new List<string>() { "ru", "en" };
            if (!cultures.Contains(cultureName))
            {
                cultureName = "en";
            }
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureName);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName);
            Thread.CurrentThread.CurrentCulture.NumberFormat = new NumberFormatInfo { CurrencyDecimalSeparator = ".", NumberDecimalSeparator = "." };
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }
    }

    public class LocalizeApiAttribute : HttpActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var cultureName = actionContext.ControllerContext.RouteData.Values.ContainsKey("lang") 
                ? actionContext.ControllerContext.RouteData.Values["lang"].ToString() 
                : "en";

            List<string> cultures = new List<string>() { "ru", "en" };
            if (!cultures.Contains(cultureName))
            {
                cultureName = "en";
            }
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureName);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName);
            Thread.CurrentThread.CurrentCulture.NumberFormat = new NumberFormatInfo { CurrencyDecimalSeparator = ".", NumberDecimalSeparator = "." };
        }
    }
}