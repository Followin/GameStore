using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Web.ModelBinders
{
    public class SessionIdModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            return controllerContext.HttpContext.Session.SessionID;
        }
    }
}