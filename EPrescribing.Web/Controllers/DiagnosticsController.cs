using EPrescribing.Web.Data.Services;
using EPrescribing.Web.Helpers;
using EPrescribing.Web.Models;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EPrescribing.Web.Controllers
{
    public class DiagnosticsController : Controller
    {
        private readonly IDiagnosticService _diagnosticService;
        private readonly Message _message = new Message();

        public DiagnosticsController()
        {
            _diagnosticService = new DiagnosticService();
        }

        // GET: Diagnostic
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

            var models = await _diagnosticService.GetAllPageListAsync(page.Value, NoOfRows.Value, searchString);
            return View(models);
        }

        // GET: Diagnostics/Details/5

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Diagnostic model = await _diagnosticService.FindAsync(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: Diagnostics/Create
        [AppAuthorization]
        public async Task<ActionResult> Create()
        {
            ViewBag.ParentId = new SelectList(await _diagnosticService.GetAllParentDiagnosisAsync(), "Id", "TestName");
            return View();
        }

        // POST: Diagnostics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public async Task<ActionResult> Create(Diagnostic model)
        {
            if (ModelState.IsValid)
            {
                var isExistItem = await _diagnosticService.IsExistItemAsync(model.TestName);
                if (isExistItem)
                {
                    _message.custom(this, "Diagnosis Name already exist!");
                    ViewBag.ParentId = new SelectList(await _diagnosticService.GetAllParentDiagnosisAsync(), "Id", "TestName", model.ParentId);
                    return View(model);
                }
                var created = await _diagnosticService.AddAsync(model);
                if (created)
                {
                    _message.save(this);
                    return RedirectToAction("Index");
                }

            }
            ViewBag.ParentId = new SelectList(await _diagnosticService.GetAllParentDiagnosisAsync(), "Id", "TestName", model.ParentId);
            _message.custom(this, "Invalid item!");
            return View(model);
        }

        // GET: Diagnostics/Edit/5
        [AppAuthorization]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Diagnostic model = await _diagnosticService.FindAsync(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentId = new SelectList(await _diagnosticService.GetAllParentDiagnosisAsync(), "Id", "TestName", model.ParentId);
            return View(model);
        }

        // POST: Diagnostics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public async Task<ActionResult> Edit(Diagnostic model)
        {
            if (ModelState.IsValid)
            {
                var isExistItem = await _diagnosticService.IsExistItemForUpdateAsync(model.Id, model.TestName);
                if (isExistItem)
                {
                    _message.custom(this, "Diagnosis Name already exist!");
                    ViewBag.ParentId = new SelectList(await _diagnosticService.GetAllParentDiagnosisAsync(), "Id", "TestName", model.ParentId);
                    return View(model);
                }
                var update = await _diagnosticService.UpdateAsync(model);
                if (update)
                {
                    _message.update(this);
                    return RedirectToAction("Index");
                }

            }
            ViewBag.ParentId = new SelectList(await _diagnosticService.GetAllParentDiagnosisAsync(), "Id", "TestName", model.ParentId);
            _message.custom(this, "Invalid data!");
            return View(model);
        }

        // GET: Diagnostics/Delete/5
        [AppAuthorization]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var delete = await _diagnosticService.DeleteAsync(id ?? 0);
            if (delete)
            {
                _message.delete(this);
                return RedirectToAction("Index");
            }
            _message.custom(this, "Can't delete item!");
            return RedirectToAction("Delete", new { id });
        }



    }
}
