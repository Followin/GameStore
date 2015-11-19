using System;
using System.Web.Mvc;
using GameStore.Static;
using GameStore.Web.Abstract;

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
            return new ViewResult() {ViewName = "CardPayment"};
        }
    }
}