using EPrescribing.Web.Data;
using EPrescribing.Web.Data.Services;
using EPrescribing.Web.Helpers;
using EPrescribing.Web.Models;
using System.Net;
using System.Web.Mvc;

namespace EPrescribing.Web.Controllers
{
    public class ContactSectionController : Controller
    {
        private AppEntities db = new AppEntities();
        private IContactSectionService _contactSectionService;
        Message _message = new Message();

        public ContactSectionController()
        {
            _contactSectionService = new ContactSectionService(db);
        }

        // GET: Designations/Index
        [AppAuthorization]
        public ActionResult Index(string currentFilter, string searchString, int? page = 1, int? NoOfRows = 10)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.page = page;
            ViewBag.CurrentFilter = searchString;
            ViewBag.NoOfRows = NoOfRows;
            return View(_contactSectionService.GetPageList(page.Value, NoOfRows.Value, searchString));

        }
        //public ActionResult Index()
        //{
        //    var list = _designationService.GetAll();
        //    return View(list.ToList());
        //}

        // GET: Departments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactSection model = _contactSectionService.GetDetails(id ?? 0);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: Departments/Create
        [AppAuthorization]
        public ActionResult Create()
        {

            return View(new ContactSection());
        }
        #region POST Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public ActionResult Create(ContactSection model)
        {
            if (ModelState.IsValid)
            {
                if (_contactSectionService.Add(model))
                {
                    //Saved Successfully;
                    _message.save(this);
                    return RedirectToAction("Index");
                }
            }
            //No Data Saved
            _message.warning(this, "Invalid Data!");
            return View(model);
        }
        #endregion

        // GET: Departments/Edit/5
        [AppAuthorization]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactSection model = _contactSectionService.GetDetails(id ?? 0);
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public ActionResult Edit(ContactSection model)
        {
            if (ModelState.IsValid)
            {
                
                if (_contactSectionService.Update(model))
                {
                    _message.update(this);
                    //Updaed Successfully;
                    return RedirectToAction("Index");
                }
            }
            //No Data Updated

            return View(model);
        }

        // GET: Departments/Delete/5
        [AppAuthorization]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (_contactSectionService.Delete(id ?? 0))
            {
                _message.delete(this);
                //Deleted Successfully;
            }
            else
            {
                _message.custom(this, "Can't delete item!");
                //No Data Deleted
            }
            return RedirectToAction("Index");
        }
    }
}
