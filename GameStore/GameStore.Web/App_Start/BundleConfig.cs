using System.Web.Optimization;

namespace GameStore.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.ripple-effect.js"));

            bundles.Add(new ScriptBundle("~/Scripts/layout").Include(
                "~/Scripts/modal.js",
                "~/Scripts/layout.js",
                "~/Scripts/hideable.js"
                ));

            bundles.Add(new ScriptBundle("~/Scripts/forms").Include(
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/jquery.autogrow-textarea.js",
                "~/Scripts/forms.js"));

            bundles.Add(new ScriptBundle("~/scripts/comments").Include(
                "~/Scripts/comments.js"));

            bundles.Add(new ScriptBundle("~/Scripts/ban").Include(
                "~/Scripts/ban.js"));

            bundles.Add(new ScriptBundle("~/Scripts/games").Include(
                "~/Scripts/games.js"));

            bundles.Add(new ScriptBundle("~/Scripts/orders").Include(
                "~/Scripts/orders.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/reset.css",
                "~/Content/normalize.css",
                "~/Content/font-awesome.css",
                "~/Content/site.css",
                "~/Content/forms.css",
                "~/Content/modal.css"));
        }
    }
}