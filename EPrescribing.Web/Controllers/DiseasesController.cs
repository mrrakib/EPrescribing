using EPrescribing.Web.Data.Services;
using EPrescribing.Web.Helpers;
using EPrescribing.Web.Models;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EPrescribing.Web.Controllers
{
    public class DiseasesController : Controller
    {
        private readonly IDiseaseService _diseaseService;
        private readonly Message _message = new Message();

        public DiseasesController()
        {
            _diseaseService = new DiseaseService();
        }

        // GET: Diseases
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

            var models = await _diseaseService.GetAllPageListAsync(page.Value, NoOfRows.Value, searchString);
            return View(models);
        }

        // GET: Diseases/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disease disease = await _diseaseService.FindAsync(id);
            if (disease == null)
            {
                return HttpNotFound();
            }
            return View(disease);
        }

        // GET: Diseases/Create
        [AppAuthorization]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Diseases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public async Task<ActionResult> Create(Disease disease)
        {
            if (ModelState.IsValid)
            {
                var isExistItem = await _diseaseService.IsExistItemAsync(disease.Name);
                if (isExistItem)
                {
                    _message.custom(this, "Disease Name already exist!");
                    return View(disease);
                }
                var created = await _diseaseService.AddAsync(disease);
                if (created)
                {
                    _message.save(this);
                    return RedirectToAction("Index");
                }

            }
            _message.custom(this, "Ivalid data!");
            return View(disease);
        }

        // GET: Diseases/Edit/5
        [AppAuthorization]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disease disease = await _diseaseService.FindAsync(id);
            if (disease == null)
            {
                return HttpNotFound();
            }
            return View(disease);
        }

        // POST: Diseases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public async Task<ActionResult> Edit(Disease disease)
        {
            if (ModelState.IsValid)
            {
                var isExistItem = await _diseaseService.IsExistItemForUpdateAsync(disease.Id, disease.Name);
                if (isExistItem)
                {
                    _message.custom(this, "Disease Name already exist!");
                    return View(disease);
                }
                var update = await _diseaseService.UpdateAsync(disease);
                if (update)
                {
                    _message.update(this);
                    return RedirectToAction("Index");
                }

            }
            _message.custom(this, "Invalid data!");
            return View(disease);
        }

        // GET: Diseases/Delete/5
        [AppAuthorization]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var delete = await _diseaseService.DeleteAsync(id ?? 0);
            if (delete)
            {
                _message.delete(this);
                return RedirectToAction("Index");
            }
            _message.custom(this, "Invalid data!");
            return RedirectToAction("Delete", new { id });
        }

    }
}
