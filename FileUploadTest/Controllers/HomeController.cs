using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileUploadTest.Controllers
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

        [HttpPost]
        public JsonResult BasicDelete(string fileid, string _method)
        {
            //刪除時自動傳入 guid 和 _method = "DELETE" (js 7778行)
            return Json(new { success = true });
        }

        [HttpPost]
        public JsonResult BasicUpload(HttpPostedFileBase uploadfile, string filename, string fileid, string other)
        {
            //response message
            //http://docs.fineuploader.com/branch/master/endpoint_handlers/traditional.html
            try
            {
                if (uploadfile == null)
                {
                    throw new FileNotFoundException();
                }

                string path = Server.MapPath("~/FileUpload");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //因為有可以會改檔名，所以要使用fineuploder的檔名
                //string filepath = Path.Combine(path, Path.GetFileName(uploadfile.FileName));
                string filepath = Path.Combine(path, filename);
                uploadfile.SaveAs(filepath);

                Dictionary<string, object> result = new Dictionary<string, object>();
                result.Add("success", true);
                result.Add("filepath", filepath);
                //改變預設傳入檔案的 guid
                //result.Add("newUuid", "abc123");

                return Json(result);
            }
            catch (Exception ex)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                result.Add("success", false);
                result.Add("error", ex.Message);
                return Json(result);
            }
        }
    }
}