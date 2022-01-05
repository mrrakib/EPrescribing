using EPrescribing.Web.Data;
using EPrescribing.Web.Data.Services;
using EPrescribing.Web.Helpers;
using EPrescribing.Web.Models;
using System.Net;
using System.Web.Mvc;

namespace EPrescribing.Web.Controllers
{
    public class DentalSchoolsController : Controller
    {
        private AppEntities db = new AppEntities();
        private IDentalSchoolService _dentalSchoolService;
        private readonly Message _message = new Message();

        public DentalSchoolsController()
        {
            _dentalSchoolService = new DentalSchoolService(db);
        }

        // GET: Departments
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

            var list = _dentalSchoolService.GetAllPageList(page.Value, NoOfRows.Value, searchString);
            return View(list);
        }

        // GET: Departments/Details/5

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DentalSchool model = _dentalSchoolService.GetDetails(id ?? 0);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: Departments/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public ActionResult Create(DentalSchool model)
        {
            if (ModelState.IsValid)
            {
                var isExist = _dentalSchoolService.IsExistItem(model.Name);
                if (isExist)
                {
                    _message.custom(this, "Dental School Name already exist!");
                    //ModelState.AddModelError("", "Dental School Name already exist!");
                    return View(model);
                }
                if (_dentalSchoolService.Add(model))
                {
                    _message.save(this);
                    //Saved Successfully;
                    return RedirectToAction("Index");
                }
            }
            //No Data Saved
            _message.custom(this, "Invalid Data.");
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
            DentalSchool model = _dentalSchoolService.GetDetails(id ?? 0);
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
        public ActionResult Edit(DentalSchool model)
        {
            if (ModelState.IsValid)
            {
                var isExist = _dentalSchoolService.IsExistItemForUpdate(model.Id, model.Name);
                if (isExist)
                {
                    _message.custom(this, "Dental School Name already exist!");
                    return View(model);
                }
                if (_dentalSchoolService.Update(model))
                {
                    _message.update(this);
                    //Updaed Successfully;
                    return RedirectToAction("Index");
                }
            }
            //No Data Updated
            _message.custom(this, "Invalid data!");
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
            if (!_dentalSchoolService.GetReferenceDoctor(id))
            {
                if (_dentalSchoolService.Delete(id ?? 0))
                {
                    _message.delete(this);
                }
                else
                {
                    _message.custom(this, "Can't delete item!");
                }
            }
            else
            {
                _message.custom(this, "Data couldn't be deleted because It reference with The Doctors!");
            }
            return RedirectToAction("Index");
        }
    }
}
