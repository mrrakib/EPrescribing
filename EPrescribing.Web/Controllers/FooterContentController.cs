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
    public class FooterContentController : Controller
    {
        private AppEntities db = new AppEntities();
        private IFooterContentService _footerContentService;
        Message _message = new Message();

        public FooterContentController()
        {
            _footerContentService = new FooterContentService(db);
        }

        // GET: Designations/Index
        [AppAuthorization]
        public ActionResult Index()
        {
            FooterContent model = _footerContentService.GetTopOne();
            if (model != null)
            {
                return View(model);
            }
            else
            {
                return View(new FooterContent());
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public ActionResult Index(FooterContent model)
        {
            if (ModelState.IsValid)
            {
                if (_footerContentService.IsExistItemForUpdate(model.Id))
                {
                    _message.custom(this, "You are not allowed to create multiple footer!");
                    return View(model);
                }

                if (model.Id > 0 && _footerContentService.Update(model))
                {
                    _message.update(this);
                    return RedirectToAction("Index");
                }
                if (model.Id == 0 && _footerContentService.Add(model))
                {
                    _message.save(this);
                    return RedirectToAction("Index");
                }
            }
            //No Data Updated
            _message.custom(this, "Invalid data!");
            return View(model);
        }
    }
}
