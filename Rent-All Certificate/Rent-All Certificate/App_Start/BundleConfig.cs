using System.Web;
using System.Web.Optimization;

namespace Rent_All_Certificate
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/Content/jsdash").Include(
                      "~/Content/dashboard/bower_components/jquery/dist/jquery.min.js",
                      "~/Content/dashboard/bower_components/bootstrap/dist/js/bootstrap.min.js",
                      "~/Content/dashboard/bower_components/metisMenu/dist/metisMenu.min.js",
                      "~/Content/dashboard/bower_components/raphael/raphael-min.js",
                      "~/Content/dashboard/bower_components/morrisjs/morris.min.js",
                      "~/Content/dashboard/js/morris-data.js",
                      "~/Content/dashboard/dist/js/sb-admin-2.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/cssdash").Include(
                      "~/Content/dashboard/bower_components/bootstrap/dist/css/bootstrap.min.css",
                      "~/Content/dashboard/bower_components/metisMenu/dist/metisMenu.min.css", 
                      "~/Content/dashboard/dist/css/sb-admin-2.css",
                      "~/Content/dashboard/bower_components/morrisjs/morris.css",
                      "~/Content/dashboard/bower_components/font-awesome/css/font-awesome.min.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
