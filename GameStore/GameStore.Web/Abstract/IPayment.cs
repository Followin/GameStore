using System;
using System.Web.Mvc;
using GameStore.Static;

namespace GameStore.Web.Abstract
{
    public interface IPayment
    {
        /// <summary>
        /// Link to image of payment
        /// </summary>
        string ImageLink { get; }

        PaymentMethod Method { get; }

        string Name { get; }

        string Description { get; }

        ActionResult Checkout();
    }
}
