using EPrescribing.Web.Data;
using EPrescribing.Web.Data.Services;
using EPrescribing.Web.Helpers;
using EPrescribing.Web.Models;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EPrescribing.Web.Controllers
{
    public class AboutSectionsController : Controller
    {
        private AppEntities db = new AppEntities();
        private IAboutSectionService _aboutSectionService;
        Message _message = new Message();
        private readonly string uploadFileName = "about_us";
        private readonly string subPath = @"~/Content/Upload/images";

        public AboutSectionsController()
        {
            _aboutSectionService = new AboutSectionService(db);
        }

        // GET: Designations/Index
        [AppAuthorization]
        public ActionResult Index()
        {
            AboutSection model = _aboutSectionService.GetTopOne();
            if (model != null)
            {
                return View(model);
            }
            else
            {
                return View(new AboutSection());
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public ActionResult Index(AboutSection model)
        {
            if (ModelState.IsValid)
            {
                if (_aboutSectionService.IsExistItemForUpdate(model.Id))
                {
                    _message.custom(this, "You are not allowed to create multiple about us section!");
                    return View(model);
                }

                #region Delete previously uploaded image if there is any
                if (Request.Files.Count > 0)
                {
                    var hasFile = Request.Files[0];
                    if (hasFile != null && hasFile.ContentLength > 0)
                    {
                        bool exists = Directory.Exists(Server.MapPath(subPath));
                        if (exists)
                        {
                            var filteredByFilename = Directory
                                    .GetFiles(Server.MapPath(subPath))
                                    .Select(f => Path.GetFileName(f))
                                    .Where(f => f.StartsWith(uploadFileName));

                            if (filteredByFilename != null)
                            {
                                foreach (var filname in filteredByFilename)
                                {
                                    var path = Path.Combine(Server.MapPath(subPath), filname);
                                    if (System.IO.File.Exists(path))
                                    {
                                        System.IO.File.Delete(path);
                                    }
                                }

                            }
                        }

                        model.ImagePath = UploadImage(Request);
                    }

                    
                }
                #endregion


                if (model.Id > 0 && _aboutSectionService.Update(model))
                {
                    _message.update(this);
                    return RedirectToAction("Index");
                }
                if (model.Id == 0 && _aboutSectionService.Add(model))
                {
                    _message.save(this);
                    return RedirectToAction("Index");
                }
            }
            //No Data Updated
            _message.custom(this, "Invalid data!");
            return View(model);
        }

        #region Image Upload
        private string UploadImage(HttpRequestBase httpRequest)
        {
            string filePath = "";
            if (httpRequest.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    string fileExtension = System.IO.Path.GetExtension(Request.Files["image"].FileName);
                    if (fileExtension == ".png" || fileExtension == ".jpg" || fileExtension == ".jpeg")
                    {
                        var fileExt = Path.GetExtension(file.FileName);
                        string fileName = uploadFileName + fileExt;
                        
                        bool exists = Directory.Exists(Server.MapPath(subPath));
                        if (!exists)
                            Directory.CreateDirectory(Server.MapPath(subPath));

                        var path = Path.Combine(Server.MapPath(subPath), uploadFileName + fileExt);
                        file.SaveAs(path);
                        filePath = subPath + "/" + uploadFileName + fileExt;
                    }
                }
            }
            return filePath;
        }
        #endregion
    }
}
