using EPrescribing.Web.Data.Services;
using EPrescribing.Web.Helpers;
using EPrescribing.Web.Models;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EPrescribing.Web.Controllers
{
    public class GenericsController : Controller
    {
        private readonly IGenericService _genericService;
        private readonly Message _message = new Message();

        public GenericsController()
        {
            _genericService = new GenericService();
        }
        // GET: Generics
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

            var models = await _genericService.GetAllPageListAsync(page.Value, NoOfRows.Value, searchString);
            return View(models);
        }

        // GET: Generics/Details/5

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Generic generic = await _genericService.FindAsync(id);
            if (generic == null)
            {
                return HttpNotFound();
            }
            return View(generic);
        }

        // GET: Generics/Create
        [AppAuthorization]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Generics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public async Task<ActionResult> Create(Generic generic)
        {
            if (ModelState.IsValid)
            {
                var isExistItem = await _genericService.IsExistItemAsync(generic.GenericName);
                if (isExistItem)
                {
                    _message.custom(this, "Generic Name already exist!");
                    //ModelState.AddModelError("", "Generic Name already exist!");
                    return View(generic);
                }
                var created = await _genericService.AddAsync(generic);
                if (created)
                {
                    _message.save(this);
                    return RedirectToAction("Index");
                }

            }
            _message.custom(this, "Invalid data!");
            return View(generic);
        }

        // GET: Generics/Edit/5
        [AppAuthorization]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Generic generic = await _genericService.FindAsync(id);
            if (generic == null)
            {
                return HttpNotFound();
            }
            return View(generic);
        }

        // POST: Generics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public async Task<ActionResult> Edit(Generic generic)
        {
            if (ModelState.IsValid)
            {

                var isExistItem = await _genericService.IsExistItemForUpdateAsync(generic.Id, generic.GenericName);
                if (isExistItem)
                {
                    _message.custom(this, "Generic Name already exist!");
                    return View(generic);
                }
                var update = await _genericService.UpdateAsync(generic);
                if (update)
                {
                    _message.update(this);
                    return RedirectToAction("Index");
                }

            }
            _message.custom(this, "Invalid data!");
            return View(generic);
        }

        // GET: Generics/Delete/5
        [AppAuthorization]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var delete = await _genericService.DeleteAsync(id ?? 0);
            if (delete)
            {
                _message.delete(this);
                return RedirectToAction("Index");
            }
            _message.custom(this, "Can't delete!");
            return RedirectToAction("Delete", new { id });
        }
    }
}
