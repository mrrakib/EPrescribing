using EPrescribing.Web.Data;
using EPrescribing.Web.Data.Services;
using EPrescribing.Web.Helpers;
using EPrescribing.Web.Models;
using EPrescribing.Web.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;


namespace EPrescribing.Web.Controllers
{
    public class PatientsController : Controller
    {
        private AppEntities db = new AppEntities();
        private IPatientService _patientService;
        private IPrescriptionService _prescriptionService;
        private readonly Message _message;
        private IDoctorService _doctorService;

        List<SelectListItem> SreachIdList = new List<SelectListItem>()
        {
            new SelectListItem{Value="TD",Text="Today"},
            new SelectListItem{Value="ICR",Text="Incomplete Records"},           
            new SelectListItem{Value="TM",Text="This Month"},
            new SelectListItem{Value="AR",Text="All Records"}
        };
        public PatientsController()
        {
            _patientService = new PatientService(db);
            _doctorService = new DoctorService(db);
            _prescriptionService = new PrescriptionService();
            _message = new Message();
        }

        [AppAuthorization]
        public ActionResult Index(string currentFilter, string searchString, string SearchStatus, int? page = 1, int? NoOfRows = 10)
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
            ViewBag.SearchStatusId = SearchStatus;
            if (string.IsNullOrEmpty(SearchStatus))
            {
                SearchStatus = "TD";
            }
            ViewBag.SearchStatus = new SelectList(SreachIdList, "Value", "Text", SearchStatus);



            ViewBag.NoOfRows = NoOfRows;
            return View(_patientService.GetAllPatientRecords(page.Value, NoOfRows.Value, searchString, User.GETDOCTORID(), SearchStatus));

        }

        [AppAuthorization]
        public ActionResult AllPatients(string currentFilter, string searchString,string SearchMobileNo, string CurrentMobileNo, int? page = 1, int? NoOfRows = 10)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (searchString != null || SearchMobileNo!=null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
                SearchMobileNo = CurrentMobileNo;
            }
            ViewBag.page = page;
            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentMobileNo = SearchMobileNo;
            ViewBag.NoOfRows = NoOfRows;
            return View(_patientService.GetPatientsList(page.Value, NoOfRows.Value, searchString, SearchMobileNo, User.GETDOCTORID()));

        }

        // GET: Departments/Edit/5
        [AppAuthorization]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient model = _patientService.GetDetails(id ?? 0);
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AppAuthorization]
        public ActionResult Edit(Patient model)
        {
            if (ModelState.IsValid)
            {
                var patient=_patientService.GetDetails(model.Id);
                patient.Name = model.Name;
                patient.Age = model.Age;
                patient.MobileNo = model.MobileNo;
                patient.Gender = model.Gender;
                
                if (_patientService.Update(patient))
                {
                    _message.update(this);
                    return RedirectToAction("AllPatients");
                }
            }
            //No Data Updated

            return View(model);
        }


        [HttpGet]
        [AppAuthorization]
        public ActionResult Create()
        {
            Patient patient = new Patient();
            patient.Status = "N";
            patient.TretmentDate = DateTime.Now;

            return View(patient);
        }
        [HttpPost]
        [AppAuthorization]
        public ActionResult Create(Patient model)
        {
            if (ModelState.IsValid)
            {
                var doctor = _doctorService.GetDoctorByAppUserID(User.Identity.GetUserId());
                int doctorId = doctor != null ? doctor.Id : 0;
                var count = _patientService.GetPatientSerialNoByDate(model.TretmentDate, doctorId);
                string PatientId = DateTime.Now.Year + "-" + User.GETMOBILENO().Substring(User.GETMOBILENO().Length - 4) + "-" + count;
                model.PatientID = PatientId;
                model.DoctorId = doctorId;
                if (_patientService.Add(model))
                {
                    _message.success(this, "Patient Added Successfully!");


                    return RedirectToAction("AddEditPrescriptionHistory", "Prescriptions", new { PatientIntId = model.Id, TretmentDate = model.TretmentDate });
                }
                else
                {
                    _message.custom(this, "No Data saved!");
                    return View(model);
                }
            }
            _message.custom(this, "Invalid data!");
            return View(model);
        }

        [HttpGet]
        public ActionResult GetPatient(string status, DateTime treatmentDate)
        {
            if (status == "N")
            {
                Patient patient = new Patient();
                patient.Status = status;
                patient.TretmentDate = treatmentDate;
                return View("_AddPatient", patient);
            }
            else
            {
                VMPatient patient = new VMPatient();
                patient.Status = status;
                patient.TretmentDate = treatmentDate;
                return View("_SearchPatient", patient);
            }
        }
        [HttpGet]
        public ActionResult GetExistingPatient(string PatientId, string MobileNo, DateTime treatMentDate)
        {
            List<VMExistingPatient> existingPatientList = new List<VMExistingPatient>();
            if (!string.IsNullOrEmpty(PatientId))
            {
                var patient = _patientService.GetPatientByPatientId(PatientId, User.GETDOCTORID());
                if (patient != null)
                {
                    VMExistingPatient model = new VMExistingPatient();
                    model.PatientIntId = patient.Id;
                    model.PatientID = patient.PatientID;
                    model.Name = patient.Name;
                    model.Age = patient.Age;
                    model.Gender = patient.Gender;
                    model.CreatedDate = patient.CreatedDate;
                    model.TretmentDate = treatMentDate;
                    model.LastSeenDate=_patientService.GetLastSeenDateByPatientId(model.PatientIntId);
                    existingPatientList.Add(model);
                }
            }
            else if (!string.IsNullOrEmpty(MobileNo))
            {
                var patientList = _patientService.GetPatientByMobileNo(MobileNo, User.GETDOCTORID()).ToList();
                if (patientList != null)
                {
                    foreach (var patient in patientList)
                    {
                        VMExistingPatient model = new VMExistingPatient();
                        model.PatientIntId = patient.Id;
                        model.PatientID = patient.PatientID;
                        model.Name = patient.Name;
                        model.Age = patient.Age;
                        model.Gender = patient.Gender;
                        model.CreatedDate = patient.CreatedDate;
                        model.TretmentDate = treatMentDate;
                        model.LastSeenDate = _patientService.GetLastSeenDateByPatientId(model.PatientIntId);
                        existingPatientList.Add(model);
                    }
                }
            }
            return View("_ExistingPatient", existingPatientList);
        }
        [AppAuthorization]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (_prescriptionService.Delete(id ?? 0))
            {
                //Deleted Successfully;
            }
            else
            {
                //No Data Deleted
            }
            return RedirectToAction("Index");
        }
    }
}