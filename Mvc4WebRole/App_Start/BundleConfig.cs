using System.Web.Optimization;

namespace Mvc4WebRole
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/OtomotivistBottomFix").Include(
                        "~/Scripts/jquery.sidecontent.js",
                        "~/Content/Layout.js",
                        "~/Scripts/bootstrap.min.js",
                        //"~/Scripts/bootstrap-select.min.js",
                        "~/Scripts/filter.js"
                        //"~/Scripts/jquery.bxslider.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.theme.css"
                        ));
            bundles.Add(new StyleBundle("~/Content/OtomotivistInliceStyles").Include(
                                    "~/Content/bootstrap.min.css",
                                    "~/Content/css/font-awesome.css",
                                    "~/Content/themes/base/jquery.ui.tabs.css",
                                    "~/Content/Site.css"
                                    ));

        }
    }
}
