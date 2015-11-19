using System;
using System.Web;
using System.Web.Mvc;
using GameStore.Static;
using GameStore.Web.Abstract;
using GameStore.Web.Models.Order;

namespace GameStore.Web.Concrete
{
    public class CardPayment : IPayment
    {
        public string ImageLink { get; private set; }

        public PaymentMethod Method { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public CardPayment(string imageLink, string name, string description, PaymentMethod method)
        {
            ImageLink = imageLink;
            Name = name;
            Description = description;
            Method = method;
        }

        public ActionResult Checkout()
        {
            switch (Method)
            {
                case PaymentMethod.Visa:
                    return new RedirectResult(new UrlHelper(HttpContext.Current.Request.RequestContext).Action("VisaPayment", "Bank"));
                case PaymentMethod.Mastercard:
                    return new RedirectResult(new UrlHelper(HttpContext.Current.Request.RequestContext).Action("MastercardPayment", "Bank"));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}