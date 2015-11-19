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
        String ImageLink { get; }

        PaymentMethod Method { get; }

        String Name { get; }

        String Description { get; }

        ActionResult Checkout();
    }
}
