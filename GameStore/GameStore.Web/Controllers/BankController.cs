using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BLL.BankService;
using GameStore.BLL.Commands.Order;
using GameStore.BLL.CQRS;
using GameStore.BLL.Queries.Order;
using GameStore.BLL.QueryResults.Order;
using GameStore.Static;
using GameStore.Web.App_LocalResources;
using GameStore.Web.Models;
using GameStore.Web.Models.Order;
using NLog;

namespace GameStore.Web.Controllers
{
    public class BankController : BaseController
    {
        public ActionResult VisaPayment()
        {
            var model = new CardPaymentViewModel
            {
                Method = PaymentMethod.Visa
            };
            return View("CardPayment", model);
        }

        public ActionResult MastercardPayment()
        {
            var model = new CardPaymentViewModel
            {
                Method = PaymentMethod.Mastercard
            };
            return View("CardPayment", model);
        }

        [HttpPost]
        public ActionResult Payment(CardPaymentViewModel model)
        {
            var currentOrder = QueryDispatcher.Dispatch<GetCurrentOrderQuery, OrderQueryResult>(
                new GetCurrentOrderQuery
                {
                    UserId = Int32.Parse((User as ClaimsPrincipal).FindFirst(ClaimTypes.SerialNumber).Value)
                });

            var command = Mapper.Map<CardPaymentViewModel, PerformPaymentCommand>(model);
            command.Sum = currentOrder.OrderDetails.Sum(x => x.Price*x.Quantity);

            var result = CommandDispatcher.Dispatch(command);

            switch ((PaymentResult) result.Data)
            {
                case PaymentResult.Success:
                    var checkoutCommand = new CheckoutOrderCommand() {Id = currentOrder.Id};
                    CommandDispatcher.Dispatch(checkoutCommand);
                    SuccessMessage(GlobalRes.PaymentSucceded);
                    return RedirectToAction("Index", "Game");
                case PaymentResult.Fail:
                    ErrorMessage(GlobalRes.PaymentFailed);
                    return RedirectToAction("Index", "Game");
                case PaymentResult.NotEnoughMoney:
                    ErrorMessage(GlobalRes.NotEnoughMoney);
                    return RedirectToAction("Index", "Game");
                case PaymentResult.CodeConfirmRequired:
                    return RedirectToAction("ConfirmPayment");
                case PaymentResult.CardDoesntExist:
                    ErrorMessage(GlobalRes.CardDoesntExist);
                    return RedirectToAction("Index", "Game");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public ActionResult ConfirmPayment()
        {
            return View("ConfirmPayment");
        }

        [HttpPost]
        public ActionResult ConfirmPayment(ConfirmPaymentViewModel model)
        {
            var command = new ConfirmPaymentCommand {Code = model.Code};
            var result = CommandDispatcher.Dispatch(command);

            if (result.Success)
            {
                var currentOrder = QueryDispatcher.Dispatch<GetCurrentOrderQuery, OrderQueryResult>(
                new GetCurrentOrderQuery
                {
                    UserId = Int32.Parse((User as ClaimsPrincipal).FindFirst(ClaimTypes.SerialNumber).Value)
                });
                var checkoutCommand = new CheckoutOrderCommand() { Id = currentOrder.Id };

                CommandDispatcher.Dispatch(checkoutCommand);
                SuccessMessage(GlobalRes.PaymentSucceded);
                return RedirectToAction("Index", "Game");
            }

            ModelState.AddModelError("Code", GlobalRes.WrongCode);
            return View("ConfirmPayment", model);
        }

        public BankController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ILogger logger) : base(commandDispatcher, queryDispatcher, logger)
        {
        }
    }
}