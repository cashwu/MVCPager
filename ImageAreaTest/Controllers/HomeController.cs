using ImageCrop;
using ImageCrop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageAreaTest.Controllers
{
    public class HomeController : Controller
    {
        private UploadImageService service = new UploadImageService();

        private readonly string UploadFolder = "FileUpload/Temp";

        private readonly string OriginalFolder = "FileUpload/Original";

        private readonly string CropFolder = "FileUpload/Crop";

        private string UploadPath
        {
            get
            {
                return Server.MapPath("~/" + this.UploadFolder);
            }
        }

        private string OriginalPath
        {
            get
            {
                return Server.MapPath("~/" + this.OriginalFolder);
            }
        }
        private string CropPath
        {
            get
            {
                return Server.MapPath("~/" + this.CropFolder);
            }
        }


        public ActionResult Index()
        {
            var result = this.service.FindAll();
            return View(result);
        }

        [HttpPost]
        public JsonResult Delete(string id)
        {
            var result = this.ProcessDelete(id);
            return Json(result);
        }

        private Dictionary<string, string> ProcessDelete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return MiscUtility.GetErrorMsg("No id");
            }
            else
            {
                Guid imageID = new Guid(id);
                var item = service.FindOne(imageID);

                if (item == null)
                {
                    return MiscUtility.GetErrorMsg("data not found");
                }

                try
                {
                    service.Delete(imageID);

                    if (!string.IsNullOrWhiteSpace(item.OriginalImage))
                    {
                        string fileName1 = Server.MapPath(string.Format("~/{0}/{1}", OriginalFolder, item.OriginalImage));
                        if (System.IO.File.Exists(fileName1))
                        {
                            System.IO.File.Delete(fileName1);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(item.CropImage))
                    {
                        string fileName2 = Server.MapPath(string.Format("~/{0}/{1}", CropFolder, item.CropImage));
                        if (System.IO.File.Exists(fileName2))
                        {
                            System.IO.File.Delete(fileName2);
                        }
                    }

                    return MiscUtility.GetSuccess();
                }
                catch (Exception ex)
                {
                    return MiscUtility.GetExceptionMsg(ex.Message);
                }
            }
        }

        public ActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase uploadFile)
        {
            if (uploadFile == null)
            {
                TempData["upload_result"] = "error";
                TempData["upload_msg"] = "not upload file";

                return View();
            }

            try
            {
                var jo = new Dictionary<string, string>();
                CropImageUtility cropUtils = new CropImageUtility(this.UploadPath, this.OriginalPath, string.Empty);
                jo = cropUtils.ProcessUploadImage(uploadFile);

                TempData["upload_result"] = jo["result"];
                TempData["upload_msg"] = jo["msg"];
            }
            catch (Exception ex)
            {
                TempData["upload_result"] = "Exception";
                TempData["upload_msg"] = ex.Message;
            }

            return View();
        }

        [HttpPost]
        public JsonResult Save(string fileName)
        {
            var jo = new Dictionary<string, string>();

            CropImageUtility cropUtils = new CropImageUtility(this.UploadPath, this.OriginalPath, string.Empty);
            var result = cropUtils.SaveUploadImageToOriginalFolder(fileName);

            if (!result["result"].Equals("success", StringComparison.OrdinalIgnoreCase))
            {
                jo.Add("result", result["result"]);
                jo.Add("msg", result["msg"]);

                return Json(jo);
            }

            try
            {
                UploadImage instance = new UploadImage()
                {
                    ID = Guid.NewGuid(),
                    OriginalImage = fileName,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                };

                service.Add(instance);

                jo.Add("result", "success");
                jo.Add("msg", string.Format(@"/{0}/{1}", OriginalFolder, fileName));
                jo.Add("id", instance.ID.ToString());

                return Json(jo);
            }
            catch (Exception ex)
            {
                return Json(MiscUtility.GetExceptionMsg(ex.Message));
            }
        }

        public JsonResult Cancel(string fileName)
        {
            CropImageUtility cropUtils = new CropImageUtility(this.UploadPath, this.OriginalPath, string.Empty);
            var result = cropUtils.DeleteUploadImage(fileName);
            return Json(result);
        }

        public ActionResult Crop(string id)
        {
            ViewData["ErrorMessage"] = string.Empty;
            ViewData["UploadImage_ID"] = string.Empty;
            ViewData["OriginalImage"] = string.Empty;
            ViewData["CropIamge"] = string.Empty;

            if (string.IsNullOrWhiteSpace(id))
            {
                ViewData["ErrorMessage"] = "not input id";
                return View();
            }

            Guid imageID;
            if (!Guid.TryParse(id, out imageID))
            {
                ViewData["ErrorMessage"] = "data id error";
                return View();
            }

            var instance = service.FindOne(imageID);
            if (instance == null)
            {
                ViewData["ErrorMessage"] = "data not exist";
                return View();
            }

            ViewData["UploadImage_ID"] = imageID;
            ViewData["OriginalImage"] = string.Format(@"/{0}/{1}", OriginalFolder, instance.OriginalImage);
            if (!string.IsNullOrWhiteSpace(instance.CropImage))
            {
                ViewData["CropIamge"] = string.Format(@"/{0}/{1}", CropFolder, instance.CropImage);
            }

            return View();
        }

        [HttpPost]
        public ActionResult CropImage(string id, int? x1, int? x2, int? y1, int? y2)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(id))
            {
                result = MiscUtility.GetErrorMsg("not input data id");
                return Json(result);
            }

            if (!x1.HasValue || !x2.HasValue || !y1.HasValue || !y2.HasValue)
            {
                result = MiscUtility.GetErrorMsg("image error");
                return Json(result);
            }

            Guid imgID;
            if (!Guid.TryParse(id, out imgID))
            {
                result = MiscUtility.GetErrorMsg("data id error");
                return Json(result);
            }

            var instance = service.FindOne(imgID);
            if (instance == null)
            {
                result = MiscUtility.GetErrorMsg("data not exist");
                return Json(result);
            }

            CropImageUtility cropUtils = new CropImageUtility(this.UploadPath, this.OriginalPath, this.CropPath);

            Dictionary<string, string> processResult = cropUtils.ProcessImageCrop
                (
                    instance,
                    new int[] { x1.Value, x2.Value, y1.Value, y2.Value }
                );

            if (processResult["result"].Equals("Success", StringComparison.OrdinalIgnoreCase))
            {
                //裁剪圖片檔名儲存到資料庫
                service.Update(instance.ID, processResult["CropImage"]);

                //如果有之前的裁剪圖片，則刪除
                if (!string.IsNullOrWhiteSpace(processResult["OldCropImage"]))
                {
                    cropUtils.DeleteCropImage(processResult["OldCropImage"]);
                }

                result.Add("result", "OK");
                result.Add("msg", "");
                result.Add("OriginalImage", string.Format(@"/{0}/{1}", this.OriginalFolder, processResult["OriginalImage"]));
                result.Add("CropImage", string.Format(@"/{0}/{1}", this.CropFolder, processResult["CropImage"]));
            }
            else
            {
                result.Add("result", processResult["result"]);
                result.Add("msg", processResult["msg"]);
            }

            return Json(result);
        }
    }
}