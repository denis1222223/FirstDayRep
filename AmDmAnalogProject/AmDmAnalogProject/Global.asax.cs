using AmDmAnalogProject.Environment;
using AmDmAnalogProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AmDmAnalogProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            InitializeDB();
        }

        private void InitializeDB()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            HtmlParser htmlParser = new HtmlParser();
            string url = "http://amdm.ru/chords/";
            htmlParser.Parse(db, url);
        }
    }
}
