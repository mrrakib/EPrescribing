using EPrescribing.Web.Data;
using EPrescribing.Web.Data.Services;
using EPrescribing.Web.Helpers;
using EPrescribing.Web.Models;
using System.Web.Mvc;

namespace EPrescribing.Web.Controllers
{
    public class PaymentsController : Controller
    {
        private AppEntities db = new AppEntities();
        private IPatientService _patientService;
        private IPrescriptionService _prescriptionService;
        private readonly Message _message;
        private IDoctorService _doctorService;
        private IPaymentService _paymentService;
        private ISubscribedService _subscriptionFeesService;

        public PaymentsController()
        {
            _patientService = new PatientService(db);
            _prescriptionService = new PrescriptionService();
            _message = new Message();
            _paymentService = new PaymentService();
            _doctorService = new DoctorService(db);
            _subscriptionFeesService = new SubscribedService();
        }

        // GET: Payments/Patient
        [AppAuthorization]
        public ActionResult Patients(int patientId = 0, string searchString = "", string sortingItem = "Date", int? page = 1, int? NoOfRows = 10)
        {
            if (page < 1)
            {
                page = 1;
            }
            ViewBag.PatientId = patientId;
            ViewBag.SearchString = searchString;
            ViewBag.SortingItem = sortingItem;
            ViewBag.page = page;
            ViewBag.NoOfRows = NoOfRows;

            var data = _patientService.GetPatientsPayments(patientId: patientId, doctorId: User.GETDOCTORID(), name: searchString, sorting: sortingItem, pageNo: page.Value, rowNo: NoOfRows.Value);
            return View(data);
        }

        // GET: Payments/Details
        [AppAuthorization]
        public ActionResult Details(int patientId, int? page = 1, int? NoOfRows = 10)
        {
            if (page < 1)
            {
                page = 1;
            }
            ViewBag.patientId = patientId;
            ViewBag.page = page;
            ViewBag.NoOfRows = NoOfRows;

            var data = _prescriptionService.GetPrescriptionByPatientId(patientId, User.GETDOCTORID(), page ?? 0, NoOfRows ?? 10);
            return View(data);
        }

        [HttpGet]
        [AppAuthorization]
        // GET: Payments/Fees
        public ActionResult Fees(int doctorId)
        {
            var doctorInfo = _doctorService.GetDetails(doctorId);
            if (doctorInfo is null)
            {
                ViewBag.Amount = 0;
                return View();
            }
            ViewBag.PaymentMethod = _paymentService.GetPaymentMethods();
            ViewBag.Amount = doctorInfo.Subscription != null ? doctorInfo.Subscription.Cost : 0;
            var subscriptionFees = new SubscriptionFees();
            if (doctorInfo.Subscription != null)
            {
                subscriptionFees.DoctorId = doctorInfo.Id;
                subscriptionFees.SubscriptionId = doctorInfo.Subscription.Id;
                subscriptionFees.PayableAmount = doctorInfo.Subscription.Cost;
            }
            return View(subscriptionFees);
        }

        [HttpPost]
        [AppAuthorization]
        // GET: Payments/Fees
        public ActionResult Fees(SubscriptionFees model)
        {
            ViewBag.PaymentMethod = _paymentService.GetPaymentMethods();
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var subscription = _subscriptionFeesService.Add(model);
            if (subscription is null)
                return View(model);

            var message = "Thank you for your request. You will be notified via SMS in 24-48 hours on whether your request is successful!";
            _message.success(this, message);
            return RedirectToAction("Index", "Home");
        }
    }
}
