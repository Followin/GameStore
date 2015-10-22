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
                "~/Scripts/layout.js",
                "~/Scripts/hideable.js"));

            bundles.Add(new ScriptBundle("~/Scripts/forms").Include(
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/jquery.autogrow-textarea.js",
                "~/Scripts/forms.js"));

            bundles.Add(new ScriptBundle("~/scripts/comments").Include(
                "~/Scripts/comments.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/site.css",
                "~/Content/forms.css"));
        }
    }
}