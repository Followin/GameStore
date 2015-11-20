using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Web.ModelBinders
{
    public class SessionIdValueProvider : IValueProvider
    {

        public bool ContainsPrefix(string prefix)
        {
            return string.Compare(prefix, "sessionId", StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        public ValueProviderResult GetValue(string key)
        {
            return ContainsPrefix(key)
                ? new ValueProviderResult((string) HttpContext.Current.Session.SessionID, null,
                    CultureInfo.InvariantCulture)
                : null;
        }
    }
}