using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CKEditor.Models;
using Microsoft.Security.Application;
using Newtonsoft.Json;

namespace CKEditor.Controllers
{
    public class HomeController : Controller
    {
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

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(string subject, string content)
        {
            Dictionary<string, string> jo = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(subject))
            {
                jo.Add("Msg", "沒有輸入標題");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }
            if (string.IsNullOrWhiteSpace(content))
            {
                jo.Add("Msg", "沒有輸入內容");
                return Content(JsonConvert.SerializeObject(jo), "application/json");
            }

            try
            {
                Article article = new Article();
                article.Subject = Sanitizer.GetSafeHtmlFragment(subject);
                article.Content = Sanitizer.GetSafeHtmlFragment(content);
                jo.Add("Result", "Success");
            }
            catch (Exception ex)
            {
                jo.Add("Result", "Failure");
                jo.Add("Result", ex.Message);
            }

            return Content(JsonConvert.SerializeObject(jo), "application/json");
        }
    }
}