using EPrescribing.Web.Data.Services;
using EPrescribing.Web.Helpers;
using EPrescribing.Web.Models;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EPrescribing.Web.Controllers
{
    public class MedicinesController : Controller
    {
        private readonly IMedicineService _medicineService;
        private readonly IBrandService _brandService;
        private readonly IGenericService _genericService;
        private readonly Message _message = new Message();

        public MedicinesController()
        {
            _medicineService = new MedicineService();
            _brandService = new BrandService();
            _genericService = new GenericService();
        }
        // GET: Medicines
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

            var models = await _medicineService.GetAllPageListAsync(page.Value, NoOfRows.Value, searchString);
            return View(models);
        }

        // GET: Medicines/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicine medicine = await _medicineService.FindAsync(id);
            if (medicine == null)
            {
                return HttpNotFound();
            }
            return View(medicine);
        }

        // GET: Medicines/Create
        [AppAuthorization]
        public async Task<ActionResult> Create()
        {
            var brands = await _brandService.GetAllAsync();
            var generics = await _genericService.GetAllAsync();
            var newGenerics=generics.ToList().Select(g => new
            {
                Id = g.Id,
                GenericName = g.Formulation + "." + g.GenericName + "." + g.DoseAmount
            });
            ViewBag.BrandId = new SelectList(brands, "Id", "BrandName");
            ViewBag.GenericId = new SelectList(newGenerics, "Id", "GenericName");
            return View();
        }

        // POST: Medicines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public async Task<ActionResult> Create(Medicine medicine)
        {
            if (ModelState.IsValid)
            {
                var isExistItem = await _medicineService.IsExistItemAsync(medicine.Name);
                if (isExistItem)
                {
                    _message.custom(this, "Medicine Name already exist!");
                    return View(medicine);
                }
                var created = await _medicineService.AddAsync(medicine);
                if (created)
                {
                    _message.save(this);
                    return RedirectToAction("Index");
                }
                    
            }

            var brands = await _brandService.GetAllAsync();
            var generics = await _genericService.GetAllAsync();
            var newGenerics = generics.ToList().Select(g => new
            {
                Id = g.Id,
                GenericName = g.Formulation + "." + g.GenericName + "." + g.DoseAmount
            });
            ViewBag.BrandId = new SelectList(brands, "Id", "BrandName");
            ViewBag.GenericId = new SelectList(newGenerics, "Id", "GenericName", medicine.GenericId);
            _message.custom(this, "Invalid data!");
            return View(medicine);
        }

        // GET: Medicines/Edit/5
        [AppAuthorization]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicine medicine = await _medicineService.FindAsync(id);
            if (medicine == null)
            {
                return HttpNotFound();
            }
            var brands = await _brandService.GetAllAsync();
            var generics = await _genericService.GetAllAsync();
            var newGenerics = generics.ToList().Select(g => new
            {
                Id = g.Id,
                GenericName = g.Formulation + "." + g.GenericName + "." + g.DoseAmount
            });
            ViewBag.BrandId = new SelectList(brands, "Id", "BrandName", medicine.BrandId);
            ViewBag.GenericId = new SelectList(newGenerics, "Id", "GenericName", medicine.GenericId);

            return View(medicine);
        }

        // POST: Medicines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public async Task<ActionResult> Edit(Medicine medicine)
        {
            if (ModelState.IsValid)
            {
                var isExistItem = await _medicineService.IsExistItemForUpdateAsync(medicine.Id, medicine.Name);
                if (isExistItem)
                {
                    _message.custom(this, "Medicine Name already exist!");
                    return View(medicine);
                }
                var update = await _medicineService.UpdateAsync(medicine);
                if (update)
                {
                    _message.update(this);
                    return RedirectToAction("Index");
                }
                    
            }
            var brands = await _brandService.GetAllAsync();
            var generics = await _genericService.GetAllAsync();
            var newGenerics = generics.ToList().Select(g => new
            {
                Id = g.Id,
                GenericName = g.Formulation + "." + g.GenericName + "." + g.DoseAmount
            });
            ViewBag.BrandId = new SelectList(brands, "Id", "BrandName", medicine.BrandId);
            ViewBag.GenericId = new SelectList(newGenerics, "Id", "GenericName", medicine.GenericId);
            _message.custom(this, "Invalid data!");
            return View(medicine);
        }

        // POST: Medicines/Delete
        [AppAuthorization]
        public async Task<ActionResult> Delete(int id)
        {
            var delete = await _medicineService.DeleteAsync(id);
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
