using EPrescribing.Web.Data;
using EPrescribing.Web.Data.Services;
using EPrescribing.Web.Helpers;
using EPrescribing.Web.Models;
using System.Net;
using System.Web.Mvc;

namespace EPrescribing.Web.Controllers
{
    public class SubscriptionsController : Controller
    {
        private AppEntities db = new AppEntities();
        private ISubscriptionService _subscriptionService;
        private readonly Message _message = new Message();

        public SubscriptionsController()
        {
            _subscriptionService = new SubscriptionService(db);
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

            var list = _subscriptionService.GetAllPageList(page.Value, NoOfRows.Value, searchString);
            return View(list);
        }

        // GET: Departments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subscription model = _subscriptionService.GetDetails(id ?? 0);
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

            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public ActionResult Create(Subscription model)
        {
            if (ModelState.IsValid)
            {
                var isExist = _subscriptionService.IsExistItem(model.Name);
                if (isExist)
                {
                    _message.custom(this, "Subscription Name already exist!");
                    return View(model);
                }
                if (_subscriptionService.Add(model))
                {
                    _message.save(this);
                    return RedirectToAction("Index");
                }
            }
            //No Data Saved
            _message.custom(this, "Invalid data!");
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
            Subscription model = _subscriptionService.GetDetails(id ?? 0);
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
        public ActionResult Edit(Subscription model)
        {
            if (ModelState.IsValid)
            {
                var isExist = _subscriptionService.IsExistItemForUpdate(model.Id, model.Name);
                if (isExist)
                {
                    _message.custom(this, "Subscription Name already exist!");
                    return View(model);
                }
                if (_subscriptionService.Update(model))
                {
                    _message.update(this);
                    return RedirectToAction("Index");
                }
            }
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
            if (_subscriptionService.Delete(id ?? 0))
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
