using System;
using System.Web.Mvc;
using GameStore.Web.Abstract;

namespace GameStore.Web.Concrete
{
    public class VisaPayment : IPayment
    {
        public string ImageLink { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public VisaPayment(string imageLink, string name, string description)
        {
            ImageLink = imageLink;
            Name = name;
            Description = description;
        }

        public ActionResult Checkout()
        {
            return new ViewResult() {ViewName = "VisaPayment"};
        }
    }
}