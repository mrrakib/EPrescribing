using EPrescribing.Web.Data;
using EPrescribing.Web.Data.Services;
using EPrescribing.Web.Helpers;
using EPrescribing.Web.Models;
using System.Net;
using System.Web.Mvc;

namespace EPrescribing.Web.Controllers
{
    public class DepartmentsController : Controller
    {
        private AppEntities db = new AppEntities();
        private ICompanyService _companyService;
        private IDepartmentService _departmentService;
        private readonly Message _message = new Message();

        public DepartmentsController()
        {
            _companyService = new CompanyService(db);
            _departmentService = new DepartmentService(db);
        }

        // GET: Departments
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

            var departments = _departmentService.GetAllPageList(page.Value, NoOfRows.Value, searchString);
            return View(departments);
        }

        // GET: Departments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = _departmentService.GetDetails(id ?? 0);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // GET: Departments/Create
        public ActionResult Create()
        {
            ViewBag.CompanyId = new SelectList(_companyService.GetAll(), "Id", "CompanyName");
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                var isExist = _departmentService.IsExistItem(department.Name);
                if (isExist)
                {
                    ViewBag.CompanyId = new SelectList(_companyService.GetAll(), "Id", "CompanyName", department.CompanyId);
                    _message.custom(this, "Department Name already exist!");
                    return View(department);
                }
                if (_departmentService.Add(department))
                {
                    _message.save(this);
                    return RedirectToAction("Index");
                }
            }
            _message.custom(this, "Invalid data!");
            ViewBag.CompanyId = new SelectList(_companyService.GetAll(), "Id", "CompanyName", department.CompanyId);
            return View(department);
        }

        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = _departmentService.GetDetails(id ?? 0);
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(_companyService.GetAll(), "Id", "CompanyName", department.CompanyId);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Department department)
        {
            if (ModelState.IsValid)
            {
                var isExist = _departmentService.IsExistItemForUpdate(department.Id, department.Name);
                if (isExist)
                {
                    _message.custom(this, "Department Name already exist!");
                    ViewBag.CompanyId = new SelectList(_companyService.GetAll(), "Id", "CompanyName", department.CompanyId);
                    return View(department);
                }

                if (_departmentService.Update(department))
                {
                    _message.update(this);
                    return RedirectToAction("Index");
                }
            }
            _message.custom(this, "Invalid data!");
            ViewBag.CompanyId = new SelectList(_companyService.GetAll(), "Id", "CompanyName", department.CompanyId);
            return View(department);
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (_departmentService.Delete(id ?? 0))
            {
                _message.delete(this);
            }
            else
            {
                _message.custom(this, "Can't delete item!");
            }
            return RedirectToAction("Index");
        }
    }
}
