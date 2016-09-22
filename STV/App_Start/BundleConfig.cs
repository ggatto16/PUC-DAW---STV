using System.Web;
using System.Web.Optimization;

namespace STV
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.unobtrusive*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/ErrorHandler").Include(
                        "~/Scripts/ErrorHandler.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/fileinput").Include(
                "~/Scripts/fileinput/fileinput.min.js", 
                "~/Scripts/fileinput/pt-BR.js"));

            bundles.Add(new ScriptBundle("~/bundles/fileinputSetup").Include(
                        "~/Scripts/fileinput/fileinputSetup.js"));

            bundles.Add(new StyleBundle("~/Content/fileinput").Include("~/Content/fileinput.min.css"));

            bundles.Add(new StyleBundle("~/Content/stars").Include(
                      "~/Content/star-rating.css",
                      "~/Content/star-off.svg",
                      "~/Content/star-on.svg"));
        }
    }
}
