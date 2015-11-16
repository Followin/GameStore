using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using GameStore.Auth;
using GameStore.Maps;
using GameStore.Web.Concrete;
using GameStore.Web.ModelBinders;
using GameStore.Web.Utils;
using MotorDepot.WEB.Utils;

namespace GameStore.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ModelMetadataProviders.Current = new MyMetadataProvider();
            ValueProviderFactories.Factories.Add(new SessionIdValueProviderFactory());
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;

            ClientDataTypeModelValidatorProvider.ResourceClassKey = "Validation";
            DefaultModelBinder.ResourceClassKey = "Validation";

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new BLLProfile());
                cfg.AddProfile(new WebMapperProfile());
                cfg.AddProfile(new DALProfile());
            });
            InitializePayments();

            
            //Mapper.AssertConfigurationIsValid();
        }

        public override void Init()
        {
            base.Init();
            var authModule = new ClaimBasedAuthenticationModule(type => DependencyResolver.Current.GetService(type));
            authModule.Init(this);
        }


        private void InitializePayments()
        {
            PaymentList.PaymentMethods.Add(
                "visa",
                new VisaPayment(
                    @"https://upload.wikimedia.org/wikipedia/commons/thumb/5/5e/Visa_Inc._logo.svg/200px-Visa_Inc._logo.svg.png",
                    "VISA",
                    "Payment by VISA card"));
            PaymentList.PaymentMethods.Add(
                "ibox",
                new IboxPayment(
                    @"http://www.uic.in.ua/wp-content/uploads/2014/06/ibox.png",
                    "IBOX",
                    "Payment by IBOX terminal"));
            PaymentList.PaymentMethods.Add(
                "bank",
                new BankPayment(
                    @"http://goodlogo.com/images/logos/state_bank_of_india_logo_3898.png",
                    "Bank",
                    "Invoice payment using bank"));
        }

    }
}