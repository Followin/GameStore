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
            command.Sum = currentOrder.OrderDetails.Sum(x => x.Price*x.Price);

            var result = CommandDispatcher.Dispatch(command);

            switch ((PaymentResult) result.Data)
            {
                case PaymentResult.Success:
                    return RedirectToAction("Index", "Game");
                case PaymentResult.Fail:
                    throw new NotImplementedException();
                    break;
                case PaymentResult.NotEnoughMoney:
                    throw new NotImplementedException();
                    break;
                case PaymentResult.CodeConfirmRequired:
                    return ConfirmPayment();
                case PaymentResult.CardDoesntExist:
                    throw new NotImplementedException();
                    break;
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
                return View("SuccessfulPayment");
            }

            ModelState.AddModelError("", "WRONG!!");
            return View("ConfirmPayment", model);
        }

        public BankController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ILogger logger) : base(commandDispatcher, queryDispatcher, logger)
        {
        }
    }
}