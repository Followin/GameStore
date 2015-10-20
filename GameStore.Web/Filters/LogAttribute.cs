using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using NLog;

namespace GameStore.Web.Filters
{
    public class LogAttribute : ActionFilterAttribute
    {
        private Logger logger;
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            logger = LogManager.GetLogger(filterContext.Controller.GetType().FullName);
            var eventInfo = new LogEventInfo(
                LogLevel.Info,
                logger.Name, 
                filterContext.Controller.GetType().Name + "." + filterContext.ActionDescriptor.ActionName);
            logger.Log(eventInfo);
        }
    }
}
