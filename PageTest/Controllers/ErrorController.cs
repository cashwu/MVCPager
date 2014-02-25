using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PageTest.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/
        public ActionResult Index(string error)
        {
            ViewData["Title"] = "抱歉, 處理你的請求發生錯誤";
            ViewData["Description"] = error;
            return View();
        }

        public ActionResult GenericError(string error)
        {
            ViewData["Title"] = "抱歉, 處理你的請求發生錯誤";
            ViewData["Description"] = error;
            return View();
        }

        public ActionResult Forbidden(string error)
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult PageNotFound(string error)
        {
            ViewData["Title"] = "抱歉, 處理你的請求發生404錯誤";
            ViewData["Description"] = error;
            Response.StatusCode = 404;
            return View();
        }

        public ActionResult InternalError(string error)
        {
            ViewData["Title"] = "抱歉, 處理你的請求發生500錯誤";
            ViewData["Description"] = error;
            Response.StatusCode = 500;
            return View();
        }
	}
}