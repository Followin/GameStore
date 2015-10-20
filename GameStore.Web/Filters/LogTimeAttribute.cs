using System.Diagnostics;
using System.Web.Mvc;
using NLog;

namespace GameStore.Web.Filters
{
    public class LogTimeAttribute : ActionFilterAttribute
    {
        private Stopwatch watch;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            watch = Stopwatch.StartNew();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            watch.Stop();
            var logger = LogManager.GetCurrentClassLogger();
            var eventInfo = new LogEventInfo(LogLevel.Trace, logger.Name, "Service request time: " + watch.ElapsedMilliseconds + "ms");
            eventInfo.Properties["action"] = filterContext.ActionDescriptor.ActionName;
            eventInfo.Properties["controller"] = filterContext.Controller.GetType().Name;
            logger.Log(eventInfo);
        }
    }
}
