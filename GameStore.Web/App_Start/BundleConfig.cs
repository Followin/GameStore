using System.Web.Optimization;

namespace GameStore.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/scripts/comments").Include(
                "~/Scripts/comments.js"));
        }
    }
}