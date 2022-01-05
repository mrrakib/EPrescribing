using EPrescribing.Web.Data.Services;
using EPrescribing.Web.Helpers;
using EPrescribing.Web.Models;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EPrescribing.Web.Controllers
{
    public class TreatmentsController : Controller
    {
        private readonly ITreatmentService _treatmentService;
        private readonly Message _message = new Message();

        public TreatmentsController()
        {
            _treatmentService = new TreatmentService();
        }

        // GET: Treatments
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

            var models = await _treatmentService.GetAllPageListAsync(page.Value, NoOfRows.Value, searchString);
            return View(models);
        }

        // GET: Treatments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Treatment treatment = await _treatmentService.FindAsync(id);
            if (treatment == null)
            {
                return HttpNotFound();
            }
            return View(treatment);
        }

        // GET: Treatments/Create
        [AppAuthorization]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Treatments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public async Task<ActionResult> Create(Treatment treatment)
        {
            if (ModelState.IsValid)
            {

                var isExistItem = await _treatmentService.IsExistItemAsync(treatment.Name);
                if (isExistItem)
                {
                    _message.custom(this, "Treatment Name already exist!");
                    return View(treatment);
                }
                var created = await _treatmentService.AddAsync(treatment);
                if (created)
                {
                    _message.save(this);
                    return RedirectToAction("Index");
                }

            }
            _message.custom(this, "Invalid data!");
            return View(treatment);
        }

        // GET: Treatments/Edit/5
        [AppAuthorization]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Treatment treatment = await _treatmentService.FindAsync(id);
            if (treatment == null)
            {
                return HttpNotFound();
            }
            return View(treatment);
        }

        // POST: Treatments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public async Task<ActionResult> Edit(Treatment treatment)
        {
            if (ModelState.IsValid)
            {
                var isExistItem = await _treatmentService.IsExistItemForUpdateAsync(treatment.Id, treatment.Name);
                if (isExistItem)
                {
                    _message.custom(this, "Treatment Name already exist!");
                    return View(treatment);
                }
                var update = await _treatmentService.UpdateAsync(treatment);
                if (update)
                {
                    _message.update(this);
                }
                return RedirectToAction("Index");
            }
            _message.custom(this, "Invalid data!");
            return View(treatment);
        }

        // GET: Treatments/Delete/5
        [AppAuthorization]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var delete = await _treatmentService.DeleteAsync(id ?? 0);
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
