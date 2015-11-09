using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.Web.Abstract;
using GameStore.Web.Models.Order;

namespace GameStore.Web.Concrete
{
    public class IboxPayment : IPayment
    {
        public string ImageLink { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public IboxPayment(string imageLink, string name, string description)
        {
            ImageLink = imageLink;
            Name = name;
            Description = description;
        }

        public ActionResult Checkout()
        {
            var model = new IboxPaymentViewModel {UserId = HttpContext.Current.Session.SessionID, OrderId = 1};
            var viewData = new ViewDataDictionary {Model = model};
            return new ViewResult() { ViewName = "IboxPayment", ViewData = viewData };
        }
    }
}