using System;
using System.Diagnostics;
using System.Web.Mvc;
using NLog;

namespace GameStore.Web.Filters
{
    public class ExceptionLogger : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var logger = LogManager.GetLogger(filterContext.Controller.GetType().FullName);
            var errorMessage = filterContext.RouteData.Values["controller"] 
                               + "." + filterContext.RouteData.Values["action"]
                               + filterContext.Exception.StackTrace.Split('\n')[0];
            logger.Error(filterContext.Exception, errorMessage);
                
            filterContext.ExceptionHandled = true;
            filterContext.Result = new ContentResult { Content = "Disaster has occured" };
        }
    }
}
