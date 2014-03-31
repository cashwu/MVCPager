using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileUploadTest.Models;
using System.IO;

namespace FileUploadTest.Controllers
{
    public class CarrerController : Controller
    {
        //
        // GET: /Carrer/
        public ActionResult SubmitCV()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SubmitCV(Carrer c, HttpPostedFileBase file)
        {
            if (file == null)
            {
                ModelState.AddModelError("CustomError", "Please select CV");
                return View();
            }

            if (!(file.ContentType == "application/vnd.openxmlformats-officedocument."
                || file.ContentType == "application/pdf"))
            {
                ModelState.AddModelError("CustomError", "Only .docx and .pdf file allowed ");
                return View();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

                    string path = Server.MapPath("~/UploadedCV");

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    file.SaveAs(Path.Combine(path, fileName));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("CustomError", ex);
                }
            }

            return View();
        }
	}
}