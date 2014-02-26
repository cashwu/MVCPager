using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JsonTest.Models;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using JsonTest.Helper;

namespace JsonTest.Controllers
{
    public class HomeController : Controller
    {
        NorthwindEntities db = new NorthwindEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Product(int? id)
        {
            if (!id.HasValue)
            {
                Dictionary<string, string> jo = new Dictionary<string, string>();
                jo.Add("Msg", "please input product id");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }
            else
            {
                var product = db.Products.Single(a => a.ProductID == id.Value);
                string jsonContent = new EFJavaScriptSerializer().Serialize(product);

                return new ContentResult { Content = jsonContent, ContentType = "application/json" };
            }
        }
    }
}