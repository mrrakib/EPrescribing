using EPrescribing.Web.Data;
using EPrescribing.Web.Data.Services;
using EPrescribing.Web.Helpers;
using EPrescribing.Web.Models;
using System.Net;
using System.Web.Mvc;

namespace EPrescribing.Web.Controllers
{
    public class CompaniesController : Controller
    {
        private AppEntities db = new AppEntities();
        private ICompanyService _companyService;
        Message _message = new Message();

        public CompaniesController()
        {
            _companyService = new CompanyService(db);
        }

        // GET: Companies
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

            return View(_companyService.GetCompanyPageList(page.Value, NoOfRows.Value, searchString));
        }

        // GET: Companies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = _companyService.GetDetails(id ?? 0);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                var isExist = _companyService.IsExistItem(company.CompanyName);
                if (isExist)
                {
                    _message.custom(this, "Company Name already exist!");
                    return View(company);
                }

                if (_companyService.Add(company))
                {
                    _message.save(this);
                    //Saved Successfully;
                    return RedirectToAction("Index");
                }
            }
            //No Data Saved
            return View(company);
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = _companyService.GetDetails(id ?? 0);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                var isExist = _companyService.IsExistItemForUpdate(company.Id, company.CompanyName);
                if (isExist)
                {
                    _message.custom(this, "Company Name already exist!");
                    return View(company);
                }

                if (_companyService.Update(company))
                {
                    _message.update(this);
                    //Updaed Successfully;
                    return RedirectToAction("Index");
                }

            }
            //No Data Updated
            return View(company);
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (_companyService.Delete(id ?? 0))
            {
                _message.delete(this);
            }
            else
            {
                //No Data Deleted
            }
            return RedirectToAction("Index");
        }
    }
}
