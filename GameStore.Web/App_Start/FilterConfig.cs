using System.Web.Mvc;
using GameStore.Web.Filters;
using NLog.Filters;

namespace GameStore.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
#if DEBUG
            filters.Add(new LogTimeAttribute());
#endif
            filters.Add(new LogAttribute());
            filters.Add(new ExceptionLogger());
        }
    }
}