using EPrescribing.Web.Data.Services;
using EPrescribing.Web.Helpers;
using EPrescribing.Web.Models;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EPrescribing.Web.Controllers
{
    public class TreatmentTempleteController : Controller
    {
        private readonly ITreatmentTempleteService _treatmentTempleteService;
        private readonly Message _message = new Message();

        public TreatmentTempleteController()
        {
            _treatmentTempleteService = new TreatmentTempleteService();
        }

        // GET: TreatmentTemplete
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

            var models = await _treatmentTempleteService.GetAllPageListByDoctorIdAsync(page.Value, NoOfRows.Value, searchString, User.GETDOCTORID());
            return View(models);
        }

        // GET: TreatmentTemplete/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TreatmentTemplete disease = await _treatmentTempleteService.FindAsync(id);
            if (disease == null)
            {
                return HttpNotFound();
            }
            return View(disease);
        }

        // GET: TreatmentTemplete/Create
        [AppAuthorization]
        public ActionResult Create()
        {
            return View();
        }

        // POST: TreatmentTemplete/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        [ValidateInput(false)]
        public async Task<ActionResult> Create(TreatmentTemplete model)
        {
            if (ModelState.IsValid)
            {
                var isExistItem = await _treatmentTempleteService.IsExistingTemplate(User.GETDOCTORID(), model.TreatmentName);
                if (isExistItem)
                {
                    _message.custom(this, "Name already exist!");
                    return View(model);
                }
                model.DoctorId = User.GETDOCTORID();
                var created = await _treatmentTempleteService.AddAsync(model);
                if (created)
                {
                    _message.save(this);
                    return RedirectToAction("Index");
                }
            }
            _message.custom(this, "Ivalid data!");
            return View(model);
        }

        // GET: TreatmentTemplete/Edit/5
        [AppAuthorization]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TreatmentTemplete disease = await _treatmentTempleteService.FindAsync(id);
            if (disease == null)
            {
                return HttpNotFound();
            }
            return View(disease);
        }

        // POST: TreatmentTemplete/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(TreatmentTemplete disease)
        {
            if (ModelState.IsValid)
            {
                var isExistItem = await _treatmentTempleteService.IsExistingTemplateForUpdateAsync(disease.Id, User.GETDOCTORID(), disease.TreatmentName);
                if (isExistItem)
                {
                    _message.custom(this, "Name already exist!");
                    return View(disease);
                }
                var update = await _treatmentTempleteService.UpdateAsync(disease);
                if (update)
                {
                    _message.update(this);
                    return RedirectToAction("Index");
                }

            }
            _message.custom(this, "Invalid data!");
            return View(disease);
        }

        // GET: TreatmentTemplete/Delete/5
        [AppAuthorization]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var delete = await _treatmentTempleteService.DeleteAsync(id ?? 0);
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
