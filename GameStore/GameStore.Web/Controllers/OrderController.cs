using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AutoMapper;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;
using GameStore.BLL.Queries.Order;
using GameStore.BLL.QueryResults.Order;
using GameStore.Static;
using GameStore.Web.Concrete;
using GameStore.Web.Filters;
using GameStore.Web.Models.Order;
using GameStore.Web.Properties;
using GameStore.Web.Utils;
using Microsoft.Ajax.Utilities;
using NLog;

namespace GameStore.Web.Controllers
{
    public class OrderController : BaseController
    {
        public OrderController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ILogger logger)
            : base(commandDispatcher, queryDispatcher, logger)
        {

        }

        public ActionResult Index()
        {

            var currentOrder = Mapper.Map<OrderViewModel>(QueryDispatcher.Dispatch<GetCurrentOrder, OrderQueryResult>(
                new GetCurrentOrder { UserId = 1 }));
            var orderCheckout = new OrderCheckoutViewModel
            {
                Order = currentOrder,
                PaymentMethods = PaymentList.PaymentMethods
            };
            return View(orderCheckout);
        }

        public ActionResult Details(Int32 id)
        {
            var orderResult = Mapper.Map<OrderViewModel>(QueryDispatcher.Dispatch<GetOrderByIdQuery, OrderQueryResult>(
                new GetOrderByIdQuery {Id = id}));

            return View(orderResult);
        }

        [ClaimsAuthorize(ClaimTypesExtensions.PublisherPermission, "Full")]
        public ActionResult History()
        {
            var orders =
                Mapper.Map<IEnumerable<OrderViewModel>>(QueryDispatcher
                    .Dispatch<GetOrdersHistoryQuery, OrdersQueryResult>(
                        new GetOrdersHistoryQuery()));
            return View(orders);
        }

        public ActionResult Checkout(String paymentMethodKey)
        {
            return PaymentList.PaymentMethods.ContainsKey(paymentMethodKey) 
                ? PaymentList.PaymentMethods[paymentMethodKey].Checkout() 
                : HttpNotFound();
        }

        public ActionResult Shippers()
        {
            var shippersQuery = QueryDispatcher.Dispatch<GetShippersQuery, ShippersQueryResult>(new GetShippersQuery());
            var model = new DisplayShippersViewModel
            {
                Shippers = Mapper.Map<IEnumerable<ShipperDTO>, IEnumerable<ShipperViewModel>>(shippersQuery)
            };

            return View(model);
        }


    }
}