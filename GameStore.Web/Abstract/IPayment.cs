using System;
using System.Web.Mvc;

namespace GameStore.Web.Abstract
{
    public interface IPayment
    {
        /// <summary>
        /// Link to image of payment
        /// </summary>
        String ImageLink { get; }

        String Name { get; }

        String Description { get; }

        ActionResult Checkout();
    }
}
