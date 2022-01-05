using System.Web;
using System.Web.Optimization;

namespace EPrescribing.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            #region Theme css (ODC)

            bundles.Add(new StyleBundle("~/Content/themecss").Include(
                    "~/css/bootstrap.min.css",
                    "~/css/font-awesome.min.css",
                    "~/css/meanmenu/meanmenu.min.css",
                    "~/css/jquery-confirm/jquery-confirm.min.css",
                    "~/css/toastr/toastr.min.css",
                    "~/css/animate.css",
                    "~/css/chosen/chosen.css",
                    "~/css/normalize.css",
                    "~/css/scrollbar/jquery.mCustomScrollbar.min.css",
                    "~/css/jvectormap/jquery-jvectormap-2.0.3.css",
                    "~/css/notika-custom-icon.css",
                    "~/css/wave/waves.min.css",
                    "~/css/wave/button.css",
                    "~/css/main.css",
                    "~/style.css",
                    "~/css/responsive.css"
                    
                ));
            #endregion

            #region Theme JS
            bundles.Add(new ScriptBundle("~/bundles/themejquery").Include(
                        "~/js/vendor/jquery-1.12.4.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/themebootstrap").Include(
                      "~/js/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/login").Include(
                      "~/js/login/login-action.js"));


            bundles.Add(new ScriptBundle("~/bundles/themejs").Include(
                      "~/js/jquery-price-slider.js",
                      "~/js/jquery.scrollUp.min.js",
                      "~/js/meanmenu/jquery.meanmenu.js",
                      "~/js/counterup/jquery.counterup.min.js",
                      "~/js/jcounterup/waypoints.min.js",
                      "~/js/counterup/counterup-active.js",
                      "~/js/sparkline/jquery.sparkline.min.js",
                      "~/js/sparkline/sparkline-active.js",
                      "~/js/flot/jquery.flot.js",
                      "~/js/flot/jquery.flot.resize.js",
                      "~/js/flot/curvedLines.js",
                      "~/js/flot/flot-active.js",
                      "~/js/knob/jquery.knob.js",
                      "~/js/knob/jquery.appear.js",
                      "~/js/knob/knob-active.js",
                      "~/js/wave/waves.min.js",
                      "~/js/wave/wave-active.js",
                      "~/js/jquery-confirm/jquery-confirm.min.js",
                      "~/js/toastr/toastr.min.js",
                      "~/js/wizard/jquery.bootstrap.wizard.min.js",
                      "~/js/wizard/wizard-active.js",
                      "~/js/todo/jquery.todo.js",
                      "~/js/jasny-bootstrap.min.js",
                      "~/js/chosen/chosen.jquery.js",
                      "~/js/plugins.js",
                      "~/Scripts/jquery-ui.js",
                      "~/js//main.js"

                      ));
            #endregion

            #region Front page css
            bundles.Add(new StyleBundle("~/Content/frontCss").Include(
                      "~/frontassets/css/plugin_theme_css.css",
                      "~/frontassets/css/style.css",
                      "~/frontassets/css/responsive.css"
                      ));
            #endregion

            #region Front page JS
            bundles.Add(new ScriptBundle("~/bundles/frontjs").Include(
                      "~/frontassets/js/isotope.pkgd.min.js",
                      "~/frontassets/js/slick.min.js",
                      "~/frontassets/js/jquery.appear.js",
                      "~/frontassets/js/jquery.knob.js",
                      "~/js/meanmenu/jquery.meanmenu.js",
                      "~/frontassets/js/theme-pluginjs.js",
                      "~/frontassets/js/ajax-mail.js",
                      "~/frontassets/js/theme.js"
                      ));
            #endregion

        }
    }
}
