using EPrescribing.Web.Data;
using EPrescribing.Web.Data.Services;
using EPrescribing.Web.Helpers;
using EPrescribing.Web.Models;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EPrescribing.Web.Controllers
{
    public class DoctorsController : Controller
    {
        AppEntities db = new AppEntities();
        private readonly IDoctorService _doctorService;
        Message _message = new Message();

        public DoctorsController()
        {
            _doctorService = new DoctorService(db);
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

            var models = await _doctorService.GetAllPageListAsync(page.Value, NoOfRows.Value, searchString);
            return View(models);
        }

        // GET: Brands/Details/5
        
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor brand =  _doctorService.GetDetails(id??0);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }
        public ActionResult DeactivateDoctor(int id, string currentFilter, string searchString, int? page = 1, int? NoOfRows = 10)
        {
            var doctor=_doctorService.GetDetails(id);
            doctor.IsActive = false;
            if (_doctorService.Update(doctor))
            {
                _message.success(this, "Doctor deactivated");
                return RedirectToAction("Index", new { currentFilter = currentFilter, searchString = searchString, page = page, NoOfRows = NoOfRows });
            }
            _message.custom(this, "Doctor deactivation failed");
            return RedirectToAction("Index", new { currentFilter = currentFilter, searchString = searchString, page = page, NoOfRows = NoOfRows });
        }
        public ActionResult ActivateDoctor(int id, string currentFilter, string searchString, int? page = 1, int? NoOfRows = 10)
        {
            var doctor = _doctorService.GetDetails(id);
            doctor.IsActive = true;
            if (_doctorService.Update(doctor))
            {
                _message.success(this, "Doctor activated");
                return RedirectToAction("Index", new { currentFilter = currentFilter, searchString = searchString, page = page, NoOfRows = NoOfRows });
            }
            _message.custom(this, "Doctor activation failed");
            return RedirectToAction("Index", new { currentFilter = currentFilter, searchString = searchString, page = page, NoOfRows = NoOfRows });
        }
        public ActionResult VerifyBMDCRegNo(int id, string currentFilter, string searchString, int? page = 1, int? NoOfRows = 10)
        {
            var doctor = _doctorService.GetDetails(id);
            doctor.IsBMDCVerified = !doctor.IsBMDCVerified;
            if (_doctorService.Update(doctor))
            {
                _message.success(this, "Verfication Status Updated");
                return RedirectToAction("Index", new { currentFilter = currentFilter, searchString = searchString, page = page, NoOfRows = NoOfRows });
            }
            _message.custom(this, "Failed to update");
            return RedirectToAction("Index", new { currentFilter = currentFilter, searchString = searchString, page = page, NoOfRows = NoOfRows });
        }
        
    }
}
