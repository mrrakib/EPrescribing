using EPrescribing.Web.Data;
using EPrescribing.Web.Data.Services;
using EPrescribing.Web.Helpers;
using EPrescribing.Web.Models;
using System.Net;
using System.Web.Mvc;

namespace EPrescribing.Web.Controllers
{
    public class DesignationsController : Controller
    {
        private AppEntities db = new AppEntities();
        private IDesignationService _designationService;
        Message _message = new Message();

        public DesignationsController()
        {
            _designationService = new DesignationService(db);
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
            return View(_designationService.GetDesignationPageList(page.Value, NoOfRows.Value, searchString));

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
            Designation model = _designationService.GetDetails(id ?? 0);
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

            return View(new Designation());
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public ActionResult Create(Designation model)
        {
            if (ModelState.IsValid)
            {
                var isExist = _designationService.IsExistItem(model.Name);
                if (isExist)
                {
                    _message.custom(this, "Designation Name already exist!");
                    //ModelState.AddModelError("", "Designation Name already exist!");
                    return View(model);
                }
                if (_designationService.Add(model))
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

        // GET: Departments/Edit/5
        [AppAuthorization]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Designation model = _designationService.GetDetails(id ?? 0);
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public ActionResult Edit(Designation model)
        {
            if (ModelState.IsValid)
            {
                var isExist = _designationService.IsExistItemForUpdate(model.Id, model.Name);
                if (isExist)
                {
                    _message.custom(this, "Designation Name already exist!");
                    //ModelState.AddModelError("", "Designation Name already exist!");
                    return View(model);
                }
                if (_designationService.Update(model))
                {
                    _message.update(this);
                    //Updaed Successfully;
                    return RedirectToAction("Index");
                }
            }
            //No Data Updated
            _message.warning(this, "Invalid Data!");

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
            if (_designationService.Delete(id ?? 0))
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
