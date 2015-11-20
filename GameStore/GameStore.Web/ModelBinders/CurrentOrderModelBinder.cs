using System;
using System.Web.Mvc;
using GameStore.Web.Models.Order;

namespace GameStore.Web.ModelBinders
{
    public class CurrentOrderModelBinder : IModelBinder
    {
        private const string key = "currentOrder";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var order = (OrderViewModel) controllerContext.HttpContext.Session[key];
            if (order == null)
            {
                order = new OrderViewModel();
                controllerContext.HttpContext.Session[key] = order;
            }
            return order;
        }
    }
}