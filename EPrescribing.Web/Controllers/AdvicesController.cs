using EPrescribing.Web.Data.Services;
using EPrescribing.Web.Helpers;
using EPrescribing.Web.Models;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EPrescribing.Web.Controllers
{
    public class AdvicesController : Controller
    {
        private readonly IAdviceService _adviceService;
        Message _message = new Message();

        public AdvicesController()
        {
            _adviceService = new AdviceService();
        }

        // GET: Advices
        [AppAuthorization]
        public async Task<ActionResult> Index(string currentFilter, string searchString, int? page = 1, int? NoOfRows = 10)
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

            var models = await _adviceService.GetAllPageListAsync(page.Value, NoOfRows.Value, searchString);
            return View(models);
        }

        // GET: Advices/Details/5

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advice advice = await _adviceService.FindAsync(id);
            if (advice == null)
            {
                return HttpNotFound();
            }
            return View(advice);
        }

        // GET: Advices/Create
        [AppAuthorization]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Advices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public async Task<ActionResult> Create(Advice advice)
        {
            if (ModelState.IsValid)
            {
                var isExistAdvice = await _adviceService.IsExistItemAsync(advice.AdviceName);
                if (isExistAdvice)
                {
                    _message.custom(this, "Advice Name already exist!");
                    //ModelState.AddModelError("", "Advice Name already exist!");
                    return View(advice);
                }
                var created = await _adviceService.AddAsync(advice);
                if (created)
                {
                    _message.save(this);
                    return RedirectToAction("Index");
                }


            }
            _message.custom(this, "Invalid data!");
            return View(advice);
        }

        // GET: Advices/Edit/5
        [AppAuthorization]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var advice = await _adviceService.FindAsync(id);

            if (advice == null)
            {
                return HttpNotFound();
            }
            return View(advice);
        }

        // POST: Advices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public async Task<ActionResult> Edit(Advice advice)
        {
            if (ModelState.IsValid)
            {
                var isExistAdvice = await _adviceService.IsExistItemForUpdateAsync(advice.Id, advice.AdviceName);
                if (isExistAdvice)
                {
                    _message.custom(this, "Advice Name already exist!");
                    //ModelState.AddModelError("", "Advice Name already exist!");
                    return View(advice);
                }
                var update = await _adviceService.UpdateAsync(advice);
                if (update)
                {
                    _message.update(this);
                    return RedirectToAction("Index");
                }

            }

            _message.custom(this, "Invalid data!");
            return View(advice);
        }

        // GET: Advices/Delete/5
        [AppAuthorization]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var delete = await _adviceService.DeleteAsync(id ?? 0);
            if (delete)
            {
                _message.delete(this);
                return RedirectToAction("Index");
            }
            _message.custom(this, "Can't delete item.");
            return RedirectToAction("Delete", new { id });
        }

    }
}
