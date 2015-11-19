using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AutoMapper;
using GameStore.BLL.Commands.Order;
using GameStore.BLL.CQRS;
using GameStore.BLL.DTO;
using GameStore.BLL.Queries.Order;
using GameStore.BLL.QueryResults.Order;
using GameStore.Static;
using GameStore.Web.Abstract;
using GameStore.Web.App_LocalResources;
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

        [Authorize]
        public ActionResult Order()
        {

            var currentOrder = Mapper.Map<OrderViewModel>(QueryDispatcher.Dispatch<GetCurrentOrderQuery, OrderQueryResult>(
                new GetCurrentOrderQuery
                {
                    UserId = Int32.Parse((User as ClaimsPrincipal).FindFirst(ClaimTypes.SerialNumber).Value)
                }));
            var orderCheckout = new OrderCheckoutViewModel
            {
                Order = currentOrder,
                PaymentMethods = PaymentMethodsDictionary.GetMethods().Select(x => 
                    new KeyValuePair<String, IPayment>(x.Key, PaymentList.GetPayment(x.Value)))
            };
            return View(orderCheckout);
        }

        public ActionResult Details(Int32 id)
        {
            var orderResult = Mapper.Map<OrderViewModel>(QueryDispatcher.Dispatch<GetOrderByIdQuery, OrderQueryResult>(
                new GetOrderByIdQuery { Id = id }));

            return View(orderResult);
        }

        public ActionResult Index(DateTime? minDate = null, DateTime? maxDate = null)
        {
            if (!minDate.HasValue)
            {
                minDate = DateTime.UtcNow.AddDays(-30);
            }


            var orders =
                Mapper.Map<IEnumerable<OrderViewModel>>(QueryDispatcher
                    .Dispatch<GetOrdersHistoryQuery, OrdersQueryResult>(
                        new GetOrdersHistoryQuery { MinDate = minDate, MaxDate = maxDate }));
            var model = new OrdersViewModel { Orders = orders };

            return View(model);
        }

        public ActionResult History(DateTime? minDate = null, DateTime? maxDate = null)
        {
            if (!maxDate.HasValue)
            {
                maxDate = DateTime.UtcNow.AddDays(-30);
            }

            var orders =
                Mapper.Map<IEnumerable<OrderViewModel>>(QueryDispatcher
                    .Dispatch<GetOrdersHistoryQuery, OrdersQueryResult>(
                        new GetOrdersHistoryQuery { MinDate = minDate, MaxDate = maxDate }));
            var model = new OrdersViewModel { Orders = orders };

            return View(model);
        }

        [Authorize]
        public ActionResult Checkout(String paymentMethodKey)
        {
            //var currentOrder = QueryDispatcher.Dispatch<GetCurrentOrderQuery, OrderQueryResult>(
            //    new GetCurrentOrderQuery
            //    {
            //        UserId = Int32.Parse((User as ClaimsPrincipal).FindFirst(ClaimTypes.SerialNumber).Value)
            //    });
            //CommandDispatcher.Dispatch(new CheckoutOrderCommand { Id = currentOrder.Id });

            var paymentMethodQuery = new GetPaymentMethodByKeyQuery {Key = paymentMethodKey};
            var queryResult =
                QueryDispatcher.Dispatch<GetPaymentMethodByKeyQuery, PaymentMethodQueryResult>(paymentMethodQuery);

            return PaymentList.GetPayment(queryResult.Method).Checkout();
        }

        [HttpPost]
        [ClaimsAuthorize(ClaimTypesExtensions.OrderPermission, Permissions.Edit)]
        public ActionResult Ship(Int32 id)
        {
            var command = new ShipOrderCommand { Id = id };
            var result = CommandDispatcher.Dispatch(command);

            if (Request.IsAjaxRequest())
            {
                return Json(
                    new
                    {
                        success = result.Success,
                        date = result.Success
                            ? ((DateTime)result.Data).ToShortDateString()
                            : "",
                        orderStatus = GlobalRes.Shipped
                    });
            }

            return RedirectToAction("Index");
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