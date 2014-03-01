using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JsonTest.Models;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using JsonTest.Helper;
using Newtonsoft.Json.Linq;

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

        public ActionResult Json()
        {
            return View();
        }

        public ActionResult Product()
        {
            var data = db.Products;

            JArray ja = new JArray();
            foreach (var item in data)
            {
                JObject jo = new JObject();
                jo.Add("ProductID", item.ProductID);
                jo.Add("ProductName", item.ProductName);
                jo.Add("CategoryID", item.CategoryID);
                jo.Add("CategoryName", item.Category != null ? item.Category.CategoryName : string.Empty);
                jo.Add("SupplierID", item.SupplierID);
                jo.Add("CompanyName", item.Supplier != null ? item.Supplier.CompanyName : string.Empty);
                ja.Add(jo);
            }

            return Content(JsonConvert.SerializeObject(ja), "application/json");
        }

        public ActionResult Order()
        {
            var data = db.Orders;
            JArray ja = new JArray();
            foreach (var item in data)
            {
                JObject jo = new JObject();
                jo.Add("OrderID", item.OrderID);
                jo.Add("CustomerID", item.CustomerID);
                jo.Add("EmployeeID", item.EmployeeID);
                jo.Add("OrderDate", item.OrderDate);
                jo.Add("ShippedDate", item.ShippedDate);
                jo.Add("ShipCounty", item.ShipCountry);
                ja.Add(jo);
            }

            return Content(JsonConvert.SerializeObject(ja), "application/json");
        }
    }
}