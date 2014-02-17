using System.Web;
using System.Web.Optimization;

namespace InterviewTracker
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/Content/css/*.css",
                    "~/Content/jquery-dark-blue/css/custom-theme/*.css",
                    "~/Content/font-awesome-4.0.3/css/font-awesome.css"
                ));
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                    "~/Scripts/js/*.js",
                    "~/Content/jquery-dark-blue/js/*.js"
                ));
        }
    }
}
