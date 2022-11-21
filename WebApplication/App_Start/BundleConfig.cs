using System.Web.Optimization;

namespace WebApplication
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/assets/libs/jquery/dist/jquery.min.js",
                       "~/assets/libs/jquery-validate/jquery.validate.min.js",
                       "~/assets/libs/jquery-ajax-unobtrusive/jquery.unobtrusive-ajax.min.js",
                       "~/assets/libs/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js",
                       "~/assets/libs/feather-icons/dist/feather.min.js",
                       "~/assets/libs/select2/select2.min.js",
                       "~/assets/libs/toastr/toastr.js",
                       "~/assets/js/app.js"));

            bundles.Add(new StyleBundle("~/app/css").Include(
                "~/assets/css/app.css",
                "~/assets/libs/select2/select2.min.css",
                "~/assets/libs/toastr/toastr.css"));

            BundleTable.EnableOptimizations = false;
        }
    }
}
