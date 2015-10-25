using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using GameStore.BLL.CQRS;
using GameStore.Web.Concrete;
using GameStore.Web.Models.Order;
using GameStore.Web.Properties;
using GameStore.Web.Utils;
using NLog;

namespace GameStore.Web.Controllers
{
    public class OrderController : BaseController
    {
        public OrderController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ILogger logger) : base(commandDispatcher, queryDispatcher, logger)
        {

        }

        public ActionResult Index(OrderViewModel currentOrder)
        {
            var orderCheckout = new OrderCheckoutViewModel
            {
                Order = currentOrder,
                PaymentMethods = PaymentList.PaymentMethods
            };
            return View(orderCheckout);
        }

        public ActionResult Checkout(String paymentMethodKey)
        {
            if (PaymentList.PaymentMethods.ContainsKey(paymentMethodKey))
            {
                return PaymentList.PaymentMethods[paymentMethodKey].Checkout();
            }
            return HttpNotFound();
        }
    }
}