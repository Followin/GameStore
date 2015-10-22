using System.Diagnostics;
using System.Web.Mvc;
using NLog;

namespace GameStore.Web.Filters
{
    public class LogTimeAttribute : ActionFilterAttribute
    {
        private Stopwatch _watch;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _watch = Stopwatch.StartNew();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            _watch.Stop();
            var logger = LogManager.GetCurrentClassLogger();
            var eventInfo = new LogEventInfo(LogLevel.Trace, logger.Name, "Service request time: " + _watch.ElapsedMilliseconds + "ms");
            eventInfo.Properties["action"] = filterContext.ActionDescriptor.ActionName;
            eventInfo.Properties["controller"] = filterContext.Controller.GetType().Name;
            logger.Log(eventInfo);
        }
    }
}
