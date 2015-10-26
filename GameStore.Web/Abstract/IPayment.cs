using System;
using System.Web.Mvc;

namespace GameStore.Web.Abstract
{
    public interface IPayment
    {
        String ImageLink { get; }

        String Name { get; }

        String Description { get; }

        ActionResult Checkout();
    }
}
