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
        private Logger _logger;
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _logger = LogManager.GetLogger(filterContext.Controller.GetType().FullName);
            var eventInfo = new LogEventInfo(
                LogLevel.Info,
                _logger.Name, 
                filterContext.Controller.GetType().Name + "." + filterContext.ActionDescriptor.ActionName);
            _logger.Log(eventInfo);
        }
    }
}
