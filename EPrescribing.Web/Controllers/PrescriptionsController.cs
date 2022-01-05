using EPrescribing.Web.Data;
using EPrescribing.Web.Data.Services;
using EPrescribing.Web.Enumerations;
using EPrescribing.Web.Helpers;
using EPrescribing.Web.Models;
using EPrescribing.Web.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EPrescribing.Web.Controllers
{
    public class PrescriptionsController : Controller
    {
        private AppEntities db = new AppEntities();
        private IPrescriptionService _prescriptionService;
        private IPatientService _patientService;
        private IMedicineService _medicineService;
        private IPathologieservice _pathologieservice;
        private IDiagnosticService _diagnosticService;
        private IDiseaseService _diseaseService;
        private ITreatmentService _treatmentService;
        private ICheifComplaientService _cheifComplaientService;
        private IMedicineHistoryService _medicineHistoryService;
        private readonly Message _message;
        private IAdviceService _adviceService;
        private IPaymentService _paymentService;
        private IBrandService _brandService;
        private IGenericService _genericService;
        private ITreatmentTempleteService _treatmentTempleteService;

        public PrescriptionsController()
        {
            _prescriptionService = new PrescriptionService();
            _patientService = new PatientService(db);
            _medicineService = new MedicineService();
            _pathologieservice = new Pathologieservice();
            _diagnosticService = new DiagnosticService();
            _adviceService = new AdviceService();
            _diseaseService = new DiseaseService();
            _treatmentService = new TreatmentService();
            _paymentService = new PaymentService();
            _cheifComplaientService = new CheifComplaientService();
            _medicineHistoryService = new MedicineHistoryService();
            _brandService = new BrandService();
            _genericService = new GenericService();
            _treatmentTempleteService = new TreatmentTempleteService();
            _message = new Message();
        }

        [HttpGet]
        [AppAuthorization]
        public async Task<ActionResult> AddEditPrescriptionHistory(VMExistingPatient model, int prescriptionId = 0, string tab = "", bool isContinueWithPrescription = false)
        {
            Prescription prescription = new Prescription();
            if (prescriptionId > 0)
            {
                prescription = _prescriptionService.GetPrescriptionHistory(prescriptionId);
            }
            else if (isContinueWithPrescription)
            {
                prescription = _prescriptionService.GetLastPrescriptions(patientIntId: model.PatientIntId);
                prescription.Status = null;
            }
            else
            {
                List<ChiefComplaint> chiefComplaints = new List<ChiefComplaint>();
                chiefComplaints.Add(new ChiefComplaint { PrescriptionId = prescriptionId });
                prescription.ChiefComplaints = chiefComplaints;

                List<MedicineHistory> medicineHistory = new List<MedicineHistory>();
                medicineHistory.Add(new MedicineHistory { PrescriptionId = prescriptionId });
                prescription.MedicineHistory = medicineHistory;

                prescription.PatientId = model.PatientIntId;
                prescription.DoctorId = User.GETDOCTORID();
                prescription.CreatedDate = model.TretmentDate;
            }
            ViewBag.Tab = tab;
            ViewBag.isContinueWithPrescription = isContinueWithPrescription;

            VMPatientRecord selectedPatient = _patientService.GetPatientRecordById(prescription.PatientId);
            ViewBag.PatientName = selectedPatient.PatientName;
            ViewBag.Age = selectedPatient.Age;
            ViewBag.PatientStrId = selectedPatient.PatientStringID;
            ViewBag.NamePrefix = selectedPatient.Gender == GenderEnum.Male.ToString() ? "Mr. " : selectedPatient.Gender == GenderEnum.Female.ToString() ? "Ms. " : "";
            ViewBag.PastDoneCount = _prescriptionService.GetPrescriptionDoneCountByPatientId(prescription.PatientId);
            return View(prescription);
        }

        [HttpPost]
        [AppAuthorization]
        [ValidateInput(false)]
        public ActionResult AddEditPrescriptionHistory(Prescription model, string tab = "", bool isContinueWithPrescription = false)
        {
            if (model.Status != "E" && model.Status != "C")
            {
                model.Status = "H";
            }
            var addOrUpdate = _prescriptionService.AddOrUpdatesHistory(model, isContinueWithPrescription);
            if (!addOrUpdate)
            {
                _message.custom(this, "Error! No data saved");
                return View("AddEditPrescriptionHistory", model);
            }

            _message.success(this, "Patient History Create/Updated Successfully!");

            if (!string.IsNullOrWhiteSpace(tab))
                return RedirectToAction("Preview", new { prescriptionId = model.Id });

            return RedirectToAction("Index", "Patients");
        }

        [HttpGet]
        [AppAuthorization]
        public async Task<ActionResult> EditPrescriptionExamination(int id, string tab = "")
        {
            Prescription prescription = _prescriptionService.GetPrescriptionOnExamination(id);
            ViewBag.Tab = tab;
            ViewBag.Category = await _diagnosticService.GetAllParentDiagnosisAsync();
            VMPatientRecord selectedPatient = _patientService.GetPatientRecordById(prescription.PatientId);
            ViewBag.PatientName = selectedPatient.PatientName;
            ViewBag.Age = selectedPatient.Age;
            ViewBag.PatientStrId = selectedPatient.PatientStringID;
            ViewBag.NamePrefix = selectedPatient.Gender == GenderEnum.Male.ToString() ? "Mr. " : selectedPatient.Gender == GenderEnum.Female.ToString() ? "Ms. " : "";
            ViewBag.PastDoneCount = _prescriptionService.GetPrescriptionDoneCountByPatientId(prescription.PatientId);
            return View(prescription);
        }

        [HttpPost]
        [AppAuthorization]
        [ValidateInput(false)]
        public ActionResult EditPrescriptionExamination(Prescription model, string tab = "")
        {
            if (model.Status != "C")
            {
                model.Status = "E";
            }
            var editOrCreate = _prescriptionService.UpdateOnExamination(model);
            if (!editOrCreate)
            {
                _message.custom(this, "Error! No data saved");
                return View("EditPrescriptionExamination", model);
            }

            _message.success(this, "Patient Data Updated Successfully!");

            if (!string.IsNullOrWhiteSpace(tab))
                return RedirectToAction("Preview", new { prescriptionId = model.Id });

            return RedirectToAction("Preview", new { prescriptionId = model.Id });
        }

        [HttpGet]
        [AppAuthorization]
        public ActionResult EditPrescriptionDone(int id, string tab = "")
        {
            var prescription = _prescriptionService.GetPrescriptionDone(id);
            prescription.TotalCharge = prescription.Patient.TotalCharge;
            prescription.TotalPaid = prescription.Patient.TotalPaid;
            prescription.TotalDue = prescription.Patient.TotalDue;

            var paymentInformation = _paymentService.GetPayment(prescriptionId: id);
            if (paymentInformation != null)
            {
                prescription.TreatmentCharge = paymentInformation.TreatmentCharge;
                prescription.TodayPaid = paymentInformation.PaidAmount;
                prescription.TodayDue = paymentInformation.DueAmount;

                prescription.TotalChargeBeforeThisPrescriptionPayment = prescription.TotalCharge - prescription.TreatmentCharge;
                prescription.TotalPaidBeforeThisPrescriptionPayment = prescription.TotalPaid - prescription.TodayPaid;
                prescription.TotalDueBeforeThisPrescriptionPayment = prescription.TotalDue - prescription.TodayDue;
            }
            else
            {
                prescription.TotalChargeBeforeThisPrescriptionPayment = prescription.TotalCharge;
                prescription.TotalPaidBeforeThisPrescriptionPayment = prescription.TotalPaid;
                prescription.TotalDueBeforeThisPrescriptionPayment = prescription.TotalDue;
            }

            VMPatientRecord selectedPatient = _patientService.GetPatientRecordById(prescription.PatientId);
            ViewBag.PatientName = selectedPatient.PatientName;
            ViewBag.Age = selectedPatient.Age;
            ViewBag.PatientStrId = selectedPatient.PatientStringID;
            ViewBag.NamePrefix = selectedPatient.Gender == GenderEnum.Male.ToString() ? "Mr. " : selectedPatient.Gender == GenderEnum.Female.ToString() ? "Ms. " : "";

            ViewBag.Tab = tab;
            ViewBag.PastDoneCount = _prescriptionService.GetPrescriptionDoneCountByPatientId(prescription.PatientId);
            return View(prescription);
        }

        [HttpPost]
        [AppAuthorization]
        [ValidateInput(false)]
        public ActionResult EditPrescriptionDone(Prescription model, string tab = "")
        {

            model.Status = "C";

            var editOrCreate = _prescriptionService.UpdateTreatMentDone(model);
            if (!editOrCreate)
            {
                _message.custom(this, "Error! No data saved");
                return View("EditPrescriptionDone", model);
            }

            _message.success(this, "Patient Data Updated Successfully!");

            if (!string.IsNullOrWhiteSpace(tab))
                return RedirectToAction("Preview", new { prescriptionId = model.Id });
            return RedirectToAction("Preview", new { prescriptionId = model.Id });
        }

        public ActionResult AddNewChiefComplaint(int prescriptionId, int no)
        {

            ChiefComplaint chiefComplaint = new ChiefComplaint { No = no + 1, PrescriptionId = prescriptionId };
            return View("_ChiefComplaints", chiefComplaint);
        }

        public ActionResult AddNewMedicineHistory(int prescriptionId, int no)
        {
            MedicineHistory medicineHistory = new MedicineHistory { No = no + 1, PrescriptionId = prescriptionId };
            return View("_MedicineHistory", medicineHistory);
        }

        public async Task<JsonResult> AutoCompleteBrand(string term)
        {
            var allBrnad = await _brandService.GetAllAsync();
            
            var result = allBrnad.Where(m => m.BrandName.ToLower().Contains(term.ToLower()));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> AutoCompleteGeneric(string term)
        {
            var allGeneric = await _genericService.GetAllAsync();
            var newList= allGeneric.ToList().Select(n => new
            {
                Id = n.Id,
                GenericName = n.Formulation + "." + n.GenericName + "." + n.DoseAmount
            });
            var result = newList.Where(m => m.GenericName.ToLower().Contains(term.ToLower()));
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> AutoCompleteMedicine(int genericId, string term)
        {
            var allMedicine = await _medicineService.GetAllMedicineByBrandGenericAsync(genericId);


            var result = allMedicine.Where(m => m.Name.ToLower().StartsWith(term.ToLower()));
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AddNewOnExaminationExtraOral(int prescriptionId, int no)
        {
            OnExamination newModel = new OnExamination { No = no + 1, PrescriptionId = prescriptionId, Status = OnExaminationStatusEnum.Extraoral.ToString() };
            return View("_OnExaminationsExtraOral", newModel);
        }
        public ActionResult AddNewOnExaminationIntraOral(int prescriptionId, int no)
        {
            OnExamination newModel = new OnExamination { No = no + 1, PrescriptionId = prescriptionId, Status = OnExaminationStatusEnum.Intraoral.ToString() };
            return View("_OnExaminationsIntraOral", newModel);
        }

        public async Task<ActionResult> AddNewDiagnosticTest(int prescriptionId, int no)
        {
            ViewBag.Category = await _diagnosticService.GetAllParentDiagnosisAsync();
            DiagnosticTest newModel = new DiagnosticTest { No = no + 1, PrescriptionId = prescriptionId };
            return View("_DiagnosticTests", newModel);
        }

        public ActionResult AddNewDifferentialDiagnosis(int prescriptionId, int no)
        {
            DifferentialDiagnosis newModel = new DifferentialDiagnosis { No = no + 1, PrescriptionId = prescriptionId };
            return View("_DifferentialDiagnosis", newModel);
        }

        public ActionResult AddNewTreatmentPlan(int prescriptionId, int no)
        {
            TreatmentPlan newModel = new TreatmentPlan { No = no + 1, PrescriptionId = prescriptionId };
            return View("_TreatmentPlans", newModel);
        }

        public async Task<JsonResult> AutoCompletePathology(string term)
        {
            var all = await _pathologieservice.GetAllAsync();

            var result = all.Where(m => m.Name.ToLower().Contains(term.ToLower()));
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public async Task<JsonResult> AutoCompleteDiagnosticCategory(string term)
        {
            var all = await _diagnosticService.GetAllParentDiagnosisAsync();

            var result = all.Where(m => m.TestName.ToLower().Contains(term.ToLower()));
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public async Task<JsonResult> AutoCompleteDiagnostic(int categoryId, string term)
        {
            var all = await _diagnosticService.GetAllDiagnosticByParentIdAsync(categoryId);

            var result = all.Where(m => m.TestName.ToLower().Contains(term.ToLower()));
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public async Task<JsonResult> AutoCompleteDisease(string term)
        {
            var all = await _diseaseService.GetAllAsync();

            var result = all.Where(m => m.Name.ToLower().Contains(term.ToLower()));
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public async Task<JsonResult> AutoCompleteTreatmentTemplete(string term)
        {
            var all = await _treatmentTempleteService.GetAllByDoctorIdAsync(User.GETDOCTORID());
            if (string.IsNullOrWhiteSpace(term))
            {
                return Json(all, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = all.Where(m => m.TreatmentName.ToLower().Contains(term.ToLower()));
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> AutoCompleteTreatment(string term)
        {
            var all = await _treatmentService.GetAllAsync();

            var result = all.Where(m => m.Name.ToLower().Contains(term.ToLower()));
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddNewTreatmentDone(int prescriptionId, int no)
        {
            TreatmentDone newModel = new TreatmentDone { No = no + 1, PrescriptionId = prescriptionId };
            return View("_TreatmentDones", newModel);
        }
        public ActionResult AddNewPrescribedMedicine(int prescriptionId, int no)
        {
            PrescribedMedicine newModel = new PrescribedMedicine { No = no + 1, PrescriptionId = prescriptionId };
            return View("_PrescribedMedicines", newModel);
        }
        public ActionResult AddNewPrescribedAdvice(int prescriptionId, int no)
        {
            PrescribedAdvice newModel = new PrescribedAdvice { No = no + 1, PrescriptionId = prescriptionId };
            return View("_PrescribedAdvices", newModel);
        }
        public async Task<JsonResult> AutoCompleteAdvice(string term)
        {
            var all = await _adviceService.GetAllAsync();
            if (string.IsNullOrWhiteSpace(term))
            {

                return Json(all, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = all.Where(m => m.AdviceName.ToLower().Contains(term.ToLower()));
                return Json(result, JsonRequestBehavior.AllowGet);
            }




        }

        [HttpGet]
        [AppAuthorization]
        public ActionResult Preview(int prescriptionId)
        {
            var model = _prescriptionService.GetPrescriptionForPreviewOrPdf(prescriptionId);

            ViewBag.PatientName = model.PatientInfo != null ? model.PatientInfo.Name : "N/A";
            ViewBag.Age = model.PatientInfo != null ? model.PatientInfo.Age : "00";
            ViewBag.PatientStrId = model.PatientInfo != null ? model.PatientInfo.PatientID : "0000-0000-0000";
            ViewBag.NamePrefix = model.PatientInfo.Gender == GenderEnum.Male.ToString() ? "Mr. " : model.PatientInfo.Gender == GenderEnum.Female.ToString() ? "Ms. " : "";
            ViewBag.MobileNo = model.PatientInfo != null ? model.PatientInfo.MobileNo : "00000000000";

            return View(model);
        }

        [HttpPost]
        [AppAuthorization]
        public ActionResult PastTreatmentDone(int patientId)
        {
            VMPatientRecord selectedPatient = _patientService.GetPatientRecordById(patientId);
            ViewBag.PatientName = selectedPatient.PatientName;
            ViewBag.Age = selectedPatient.Age;
            ViewBag.PatientStrId = selectedPatient.PatientStringID;
            ViewBag.NamePrefix = selectedPatient.Gender == GenderEnum.Male.ToString() ? "Mr. " : selectedPatient.Gender == GenderEnum.Female.ToString() ? "Ms. " : "";
            var result = _prescriptionService.GetAllDonePrescriptionByPatientId(patientId);
            return View(result);
        }
    }
}