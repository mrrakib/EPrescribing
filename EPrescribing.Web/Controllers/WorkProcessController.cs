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
    public class WorkProcessController : Controller
    {
        private AppEntities db = new AppEntities();
        private IWorkProcessService _workProcessService;
        Message _message = new Message();

        public WorkProcessController()
        {
            _workProcessService = new WorkProcessService(db);
        }

        // GET: Designations/Index
        [AppAuthorization]
        public ActionResult Index()
        {
            WorkProcess model = _workProcessService.GetTopOne();
            if (model != null)
            {
                return View(model);
            }
            else
            {
                return View(new WorkProcess());
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public ActionResult Index(WorkProcess model)
        {
            if (ModelState.IsValid)
            {
                if (_workProcessService.IsExistItemForUpdate(model.Id))
                {
                    _message.custom(this, "You are not allowed to create multiple work process section!");
                    return View(model);
                }

                if (model.Id > 0 && _workProcessService.Update(model))
                {
                    _message.update(this);
                    return RedirectToAction("Index");
                }
                if (model.Id == 0 && _workProcessService.Add(model))
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
