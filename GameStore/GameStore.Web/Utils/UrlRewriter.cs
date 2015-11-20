using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace GameStore.Web.Utils
{
    public static class UrlRewriter
    {
        public static RouteValueDictionary ReplaceSection(string sectionName, string newValue)
        {
            var routeData = HttpContext.Current.Request.RequestContext.RouteData;
            var resultDictionary = routeData.Values.Keys.ToDictionary(key => key,
                key => key == sectionName ? newValue : routeData.Values[key]);
            return new RouteValueDictionary(resultDictionary);
        }
    }
}