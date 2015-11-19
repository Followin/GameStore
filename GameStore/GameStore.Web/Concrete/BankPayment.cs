using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using GameStore.BLL.Services;
using GameStore.Static;
using GameStore.Web.Abstract;

namespace GameStore.Web.Concrete
{
    public class BankPayment : IPayment
    {
        public string ImageLink { get; private set; }

        public PaymentMethod Method { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public BankPayment(string imageLink, string name, string description)
        {
            ImageLink = imageLink;
            Name = name;
            Description = description;
            Method = PaymentMethod.Bank;
        }

        public ActionResult Checkout()
        {

            var result = PdfService.GenerateInvoiceFile((HttpContext.Current.User as ClaimsPrincipal).FindFirst(ClaimTypes.SerialNumber).Value);
            return new FileContentResult(result, "application/pdf");

        }
    }
}