using EPrescribing.Web.Data;
using EPrescribing.Web.Data.Services;
using EPrescribing.Web.Helpers;
using EPrescribing.Web.Models;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;

namespace EPrescribing.Web.Controllers
{
    public class SubscribedController : Controller
    {
        private AppEntities db = new AppEntities();
        private IPatientService _patientService;
        private IPrescriptionService _prescriptionService;
        private readonly Message _message;
        private IDoctorService _doctorService;
        private IPaymentService _paymentService;
        private ISubscribedService _subscriptionFeesService;
        private readonly ISubscriptionService _subscriptionService;

        public SubscribedController()
        {
            _patientService = new PatientService(db);
            _prescriptionService = new PrescriptionService();
            _message = new Message();
            _paymentService = new PaymentService();
            _doctorService = new DoctorService(db);
            _subscriptionFeesService = new SubscribedService();
            _subscriptionService = new SubscriptionService(db);
        }

        // GET: Subscribed
        [HttpGet]
        [AppAuthorization]
        public ActionResult Index(int? page = 1, int? NoOfRows = 10)
        {
            ViewBag.page = page;
            ViewBag.NoOfRows = NoOfRows;
            ViewBag.PaymentMethod = _paymentService.GetPaymentMethods();
            ViewBag.isActive = _paymentService.GetIsActiveStatus();

            var models = _subscriptionFeesService.GetAll(filter: "", accountNo: "", transactionNo: "", isActive: "", page ?? 1, NoOfRows ?? 30);
            return View(models);
        }

        [HttpPost]
        [AppAuthorization]
        public ActionResult Index(string currentFilter, string accountNo, string transactionNo, string isActive, int? page = 1, int? NoOfRows = 10)
        {
            ViewBag.page = page;
            ViewBag.CurrentFilter = currentFilter;
            ViewBag.NoOfRows = NoOfRows;
            ViewBag.accountNo = accountNo;
            ViewBag.transactionNo = transactionNo;
            ViewBag.PaymentMethod = _paymentService.GetPaymentMethods();
            ViewBag.isActive = _paymentService.GetIsActiveStatus();

            var models = _subscriptionFeesService.GetAll(filter: currentFilter, accountNo: accountNo, transactionNo: transactionNo, isActive: isActive, page ?? 1, NoOfRows ?? 30);
            return View(models);
        }

        [HttpGet]
        [AppAuthorization]
        public ActionResult ActivateDoctor(int id, string currentFilter, int? page = 1, int? NoOfRows = 10)
        {
            var doctor = _doctorService.GetDetails(id);
            doctor.IsActive = true;
            doctor.SubscribedDate = DateTime.Now;
            doctor.SubscriptionExpiredDate = DateTime.Now.AddDays(doctor.Subscription.EvaluationPeriodInDay);

            if (_doctorService.Update(doctor))
            {
                _message.success(this, "Sucessfully Doctor Activated!");
                return RedirectToAction("Index", new { currentFilter = currentFilter, page = page, NoOfRows = NoOfRows });
            }
            _message.custom(this, "Doctor activation failed");
            return RedirectToAction("Index", new { currentFilter = currentFilter, page = page, NoOfRows = NoOfRows });
        }

        // GET: Subscribed
        [HttpGet]
        [AppAuthorization]
        public ActionResult Doctors(int? page = 1, int? NoOfRows = 10)
        {

            ViewBag.page = page;
            ViewBag.NoOfRows = NoOfRows;
            ViewBag.CurrentFilter = "";

            var models = _doctorService.GetAll().OrderBy(a => a.SubscriptionExpiredDate).ToPagedList(page ?? 1, NoOfRows ?? 30);
            return View(models);
        }

        // POST: Subscribed
        [HttpPost]
        [AppAuthorization]
        public ActionResult Doctors(string SearchString = "", int? page = 1, int? NoOfRows = 10)
        {

            ViewBag.page = page;
            ViewBag.NoOfRows = NoOfRows;
            ViewBag.CurrentFilter = SearchString;

            var models = _doctorService.GetAll().AsQueryable();
            if (!string.IsNullOrWhiteSpace(SearchString))
                models = models.Where(a => a.AppUser != null && a.AppUser.UserName.Contains(SearchString)).AsQueryable();

            var data = (PagedList<Doctor>)models.OrderBy(a => a.SubscriptionExpiredDate).ToPagedList(page ?? 1, NoOfRows ?? 30);
            return View(data);
        }

        // GET: Subscribed/Cancel
        [HttpGet]
        [AppAuthorization]
        public ActionResult Cancel(int id = 0, int page = 1)
        {
            var doctorInfo = _doctorService.GetDetails(id);
            if (doctorInfo is null)
                return RedirectToAction("Doctors", new { page });

            doctorInfo.IsActive = false;
            var update = _doctorService.Update(doctorInfo);
            if (update)
                _message.success(this, "Sucessfully Cancel Current Subscription!");
            return RedirectToAction("Doctors", new { page });
        }

        // GET: Subscribed/Upgrade
        [HttpGet]
        [AppAuthorization]
        public ActionResult Upgrade(int id = 0, int page = 1)
        {
            var doctorInfo = _doctorService.GetDetails(id);
            if (doctorInfo is null)
                return RedirectToAction("Doctors", new { page });

            doctorInfo.Subscriptions = _subscriptionService.GetAll().Where(a => a.Cost > 0).ToList();
            return View(doctorInfo);
        }

        // GET: Subscribed/Upgrade
        [HttpPost]
        [AppAuthorization]
        public ActionResult Upgrade(Doctor model)
        {
            var subscription = _subscriptionService.GetDetails((int)model.SubscriptionlId);
            var doctor = _doctorService.GetDetails(model.Id);
            if (subscription is null || doctor is null)
                return RedirectToAction("Doctors", new { });

            doctor.SubscribedDate = DateTime.Now;
            doctor.SubscriptionExpiredDate = DateTime.Now.AddDays(subscription.EvaluationPeriodInDay);
            doctor.SubscriptionlId = model.SubscriptionlId;
            doctor.IsActive = true;

            var upgrade = _doctorService.Update(doctor);
            _message.success(this, "Sucessfully Upgrade Subscription!");
            return RedirectToAction("Doctors", new { });
        }
    }
}
