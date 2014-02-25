using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PageTest.Models;

namespace PageTest.Controllers
{
    public class HomeController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            logger.Trace("Track");
            logger.Debug("Debug");
            logger.Info("Info");
            logger.Warn("Warn");
            logger.Error("Error");
            logger.Fatal("Fatal");

            return View();
        }

        public ActionResult About()
        {
            try
            {
                int a = 6;
                int b = 0;
                int result = a / b;
            }
            catch (Exception ex)
            {
                logger.Fatal(LogUtility.BuildExceptionMessage(ex));
            }

            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}