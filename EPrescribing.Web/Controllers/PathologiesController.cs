using EPrescribing.Web.Data.Services;
using EPrescribing.Web.Helpers;
using EPrescribing.Web.Models;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EPrescribing.Web.Controllers
{
    public class PathologiesController : Controller
    {
        private readonly IPathologieservice _pathologieservice;
        private readonly Message _message = new Message();

        public PathologiesController()
        {
            _pathologieservice = new Pathologieservice();
        }
        // GET: Pathologies
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

            var models = await _pathologieservice.GetAllPageListAsync(page.Value, NoOfRows.Value, searchString);
            return View(models);
        }
        // GET: Pathologies/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pathology pathology = await _pathologieservice.FindAsync(id);
            if (pathology == null)
            {
                return HttpNotFound();
            }
            return View(pathology);
        }

        // GET: Pathologies/Create
        [AppAuthorization]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pathologies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public async Task<ActionResult> Create(Pathology pathology)
        {
            if (ModelState.IsValid)
            {
                var isExistItem = await _pathologieservice.IsExistItemAsync(pathology.Name);
                if (isExistItem)
                {
                    _message.custom(this, "Pathology Name already exist!");
                    return View(pathology);
                }
                var created = await _pathologieservice.AddAsync(pathology);
                if (created)
                {
                    _message.save(this);
                    return RedirectToAction("Index");
                }

            }
            _message.custom(this, "Invalid data!");
            return View(pathology);
        }

        // GET: Pathologies/Edit/5
        [AppAuthorization]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pathology pathology = await _pathologieservice.FindAsync(id);
            if (pathology == null)
            {
                return HttpNotFound();
            }
            return View(pathology);
        }

        // POST: Pathologies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public async Task<ActionResult> Edit(Pathology pathology)
        {
            if (ModelState.IsValid)
            {
                var isExistItem = await _pathologieservice.IsExistItemForUpdateAsync(pathology.Id, pathology.Name);
                if (isExistItem)
                {
                    _message.custom(this, "Pathology Name already exist!");
                    return View(pathology);
                }
                var update = await _pathologieservice.UpdateAsync(pathology);
                if (update)
                {
                    _message.update(this);
                    return RedirectToAction("Index");
                }
            }
            _message.custom(this, "Invalid data!");
            return View(pathology);
        }

        // GET: Pathologies/Delete/5
        [AppAuthorization]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var delete = await _pathologieservice.DeleteAsync(id ?? 0);
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
