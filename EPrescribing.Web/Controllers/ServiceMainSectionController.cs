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
    public class ServiceMainSectionController : Controller
    {
        private AppEntities db = new AppEntities();
        private IServiceMainSectionService _serviceMainSectionService;
        Message _message = new Message();

        public ServiceMainSectionController()
        {
            _serviceMainSectionService = new ServiceMainSectionService(db);
        }

        // GET: Designations/Index
        [AppAuthorization]
        public ActionResult Index()
        {
            ServiceMainSection model = _serviceMainSectionService.GetTopOne();
            if (model != null)
            {
                return View(model);
            }
            else
            {
                return View(new ServiceMainSection());
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public ActionResult Index(ServiceMainSection model)
        {
            if (ModelState.IsValid)
            {
                if (_serviceMainSectionService.IsExistItemForUpdate(model.Id))
                {
                    _message.custom(this, "You are not allowed to create multiple main service section!");
                    return View(model);
                }

                if (model.Id > 0 && _serviceMainSectionService.Update(model))
                {
                    _message.update(this);
                    return RedirectToAction("Index");
                }
                if (model.Id == 0 && _serviceMainSectionService.Add(model))
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
