using EPrescribing.Web.Data.Services;
using EPrescribing.Web.Helpers;
using EPrescribing.Web.Models;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EPrescribing.Web.Controllers
{
    public class BrandsController : Controller
    {
        private readonly IBrandService _brandService;
        Message _message = new Message();

        public BrandsController()
        {
            _brandService = new BrandService();
        }

        // GET: Brands
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

            var models = await _brandService.GetAllPageListAsync(page.Value, NoOfRows.Value, searchString);
            return View(models);
        }

        // GET: Brands/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = await _brandService.FindAsync(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // GET: Brands/Create
        [AppAuthorization]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Brands/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public async Task<ActionResult> Create(Brand brand)
        {
            if (ModelState.IsValid)
            {
                var isExistItem = await _brandService.IsExistItemAsync(brand.BrandName);
                if (isExistItem)
                {
                    _message.custom(this, "Brand Name already exist!");
                    //ModelState.AddModelError("", "Brand Name already exist!");
                    return View(brand);
                }
                var created = await _brandService.AddAsync(brand);
                if (created)
                {
                    _message.save(this);
                    return RedirectToAction("Index");
                }
                    
            }
            _message.warning(this, "Invalid data!");
            return View(brand);
        }

        // GET: Brands/Edit/5
        [AppAuthorization]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = await _brandService.FindAsync(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // POST: Brands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public async Task<ActionResult> Edit(Brand brand)
        {
            if (ModelState.IsValid)
            {
                var isExistItem = await _brandService.IsExistItemForUpdateAsync(brand.Id, brand.BrandName);
                if (isExistItem)
                {
                    _message.custom(this, "Brand Name already exist!");
                    return View(brand);
                }
                var update = await _brandService.UpdateAsync(brand);
                if (update)
                {
                    _message.update(this);
                    return RedirectToAction("Index");
                }
                    
            }
            _message.warning(this, "Invalid data.");
            return View(brand);
        }

        // GET: Brands/Delete/5
        [AppAuthorization]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = await _brandService.FindAsync(id ?? 0);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // POST: Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var delete = await _brandService.DeleteAsync(id);
            if (delete)
            {
                _message.delete(this);
                return RedirectToAction("Index");
            }
                
            return RedirectToAction("Delete", new { id });
        }
    }
}
