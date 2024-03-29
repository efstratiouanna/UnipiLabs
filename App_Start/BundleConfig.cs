﻿using System.Web;
using System.Web.Optimization;

namespace UnipiLabs
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //Περιλαμβάνουμε τα παρακάτω scripts στο project για να μπορέσουμε να χρησιμοποιήσουμε το full calendar του MVC5

            //Calendar css file
            bundles.Add(new StyleBundle("~/Content/fullcalendarcss").Include(
                         "~/Content/themes/jquery.ui.all.css",
                         "~/Content/fullcalendar.css"));

            //Calendar Script file
            bundles.Add(new ScriptBundle("~/bundles/fullcalendarjs").Include(
                        "~/Scripts/jquery-ui-1.10.4.min.js",
                        "~/Scripts/fullcalendar.min.js"));


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
                      "~/Content/Site.css"));
        }
    }
}
