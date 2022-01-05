using EPrescribing.Web.Data.Services;
using EPrescribing.Web.Helpers;
using EPrescribing.Web.Models;
using EPrescribing.Web.Reports;
using EPrescribing.Web.ViewModels;
using EPrescribing.Web.ViewModels.PrintPrescription;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EPrescribing.Web.Controllers
{
    public class ReportController : Controller
    {
        private readonly IPrescriptionService _prescriptionService;

        public ReportController()
        {
            _prescriptionService = new PrescriptionService();
        }

        // GET: Report
        [HttpGet]
        [AppAuthorization]
        public async Task<ActionResult> Index(int prescriptionId = 0, bool withHeader = false)
        {
            var data = _prescriptionService.GetPrescriptionForPreviewOrPdf(prescriptionId);
            if (data.DoctorInfo is null)
                return RedirectToAction("Index", " Patients");

            LocalReport aLocalReport = new LocalReport();
            aLocalReport.DataSources.Clear();
            aLocalReport.Refresh();
            aLocalReport.EnableExternalImages = true;

            if (!data.DoctorInfo.IsBMDCVerified || String.IsNullOrWhiteSpace(data.DoctorInfo.BMDCRegistrationNumber) || !data.DoctorInfo.IsActive || (data.DoctorInfo.SubscriptionExpiredDate != null && (DateTime)data.DoctorInfo.SubscriptionExpiredDate < DateTime.Now))  //For Student
                aLocalReport.ReportPath = Server.MapPath(withHeader ? "~/Reports/RxHSPrint.rdlc" : "~/Reports/RxSPrint.rdlc");
            else                                                                                                                                    //For Doctor
                aLocalReport.ReportPath = Server.MapPath(withHeader ? "~/Reports/RxHPrint.rdlc" : "~/Reports/RxPrint.rdlc");

            //aLocalReport.ReportPath = Server.MapPath("~/Reports/RxHSPrint.rdlc");     //Testing report

            var headerInfo = new List<VMHeaderInfo>()
            {
                new VMHeaderInfo(){
                    Name="Dental Hospital",
                    OrganizationName="ABC Hospital Ltd",
                    Location="Dhaka",
                    Logo = new Uri(Server.MapPath("~/Logo/rx.jpg")).AbsoluteUri,
                    RxLogo=new Uri(Server.MapPath("~/Logo/rx.png")).AbsoluteUri,
                    WatermarkLogo=new Uri(Server.MapPath("~/Logo/watermark.jpg")).AbsoluteUri
                }
            };

            var doctorInfos = new List<VMDoctorInfo>();
            doctorInfos.Add(new VMDoctorInfo()
            {
                Name = data.DoctorInfo.Name,
                Designation = data.DoctorInfo.Designation != null ? data.DoctorInfo.Designation.Name : "",
                SpecilizedDegree = null,//"BDS, MCPS,FCPS",
                Organization = data.DoctorInfo.DentalSchool != null ? data.DoctorInfo.DentalSchool.Name : "",
                ClinicName = data.DoctorInfo.ClinicName,
                ClinicAddress = data.DoctorInfo.ClinicAddress,
                BMDCRegistrationNumber = data.DoctorInfo.BMDCRegistrationNumber,
            });

            var patientInfos = new List<Patient>();
            patientInfos.Add(data.PatientInfo);

            aLocalReport.DataSources.Add(new ReportDataSource("HeaderInfo", headerInfo));
            aLocalReport.DataSources.Add(new ReportDataSource("DoctorInfo", doctorInfos));
            aLocalReport.DataSources.Add(new ReportDataSource("PatientInfo", patientInfos));

            if (data.CheifComplaients.Count > 0)
            {
                var dataChief = data.CheifComplaients.Select(a => new VMCheifComplaient() { Id = a.Id, Description = a.Description, UpperLeft = a.UpperLeft, UpperRight = a.UpperRight, BottomLeft = a.BottomLeft, BottomRight = a.BottomRight, IsShow = (a.UpperLeft || a.UpperRight || a.BottomLeft || a.BottomRight ? true : false) }).ToList();
                aLocalReport.DataSources.Add(new ReportDataSource("CheifComplaients", dataChief));
            }
            else
            {
                aLocalReport.DataSources.Add(new ReportDataSource("CheifComplaients", new List<ChiefComplaint>()));
            }

            if (data.OnExaminationIntraoral.Count > 0)
            {
                var vMOnExamination = data.OnExaminationIntraoral.Select(a => new VMOnExamination() { Id = a.Id, Description = a.Description, UpperLeft = a.UpperLeft, UpperRight = a.UpperRight, BottomLeft = a.BottomLeft, BottomRight = a.BottomRight, IsShow = (!String.IsNullOrWhiteSpace(a.UpperLeft) || !String.IsNullOrWhiteSpace(a.UpperRight) || !String.IsNullOrWhiteSpace(a.BottomLeft) || !String.IsNullOrWhiteSpace(a.BottomRight) ? true : false) }).ToList();
                aLocalReport.DataSources.Add(new ReportDataSource("OnExaminationIntraoral", vMOnExamination));
            }
            else
            {
                aLocalReport.DataSources.Add(new ReportDataSource("OnExaminationIntraoral", new List<OnExamination>()));
            }


            //if (data.OnExaminationExtraoral.Count > 0)
            //{
            //    aLocalReport.DataSources.Add(new ReportDataSource("OnExaminationExtraoral", data.OnExaminationExtraoral));
            //}
            //else
            //{
            //    aLocalReport.DataSources.Add(new ReportDataSource("OnExaminationExtraoral", new List<OnExamination>()));
            //}

            if (data.DiagnosticTests.Count > 0)
            {
                var diagnosticTest = data.DiagnosticTests.Select(a => new VMDiagnosticTest() { TestName = a.DiagnosticName }).ToList();
                aLocalReport.DataSources.Add(new ReportDataSource("DiagnosticTests", diagnosticTest));
            }
            else
            {
                aLocalReport.DataSources.Add(new ReportDataSource("DiagnosticTests", new List<VMDiagnosticTest>()));
            }
            if (data.PrescribedAdvices.Count > 0)
            {
                aLocalReport.DataSources.Add(new ReportDataSource("PrescribedAdvices", data.PrescribedAdvices));
            }
            else
            {
                aLocalReport.DataSources.Add(new ReportDataSource("PrescribedAdvices", new List<PrescribedAdvice>()));
            }
            if (data.TreatmentPlans.Count > 0)
            {
                var plans = data.TreatmentPlans.Select(a => new VMTreatmentPlan() { Id = a.Id, TreatmentName = a.TreatmentName, UpperLeft = a.UpperLeft, UpperRight = a.UpperRight, BottomLeft = a.BottomLeft, BottomRight = a.BottomRight, IsShow = (!String.IsNullOrWhiteSpace(a.UpperLeft) || !String.IsNullOrWhiteSpace(a.UpperRight) || !String.IsNullOrWhiteSpace(a.BottomLeft) || !String.IsNullOrWhiteSpace(a.BottomRight) ? true : false) }).ToList();
                aLocalReport.DataSources.Add(new ReportDataSource("TreatmentPlans", plans));
            }
            else
            {
                aLocalReport.DataSources.Add(new ReportDataSource("TreatmentPlans", new List<TreatmentPlan>()));
            }
            if (data.DifferentialDiagnosis.Count > 0)
            {
                aLocalReport.DataSources.Add(new ReportDataSource("DifferentialDiagnosis", data.DifferentialDiagnosis));
            }
            else
            {
                aLocalReport.DataSources.Add(new ReportDataSource("DifferentialDiagnosis", new List<DifferentialDiagnosis>()));
            }

            if (data.PrescribedMedicines.Count > 0)
            {
                var prescribedMedicines = data.PrescribedMedicines.Select(a => new VMPrescribedMedicine() { MedicineName = a.MedicineName, DailyDose = a.DailyDose, Dose = a.Days, Notes = a.Notes }).ToList();
                aLocalReport.DataSources.Add(new ReportDataSource("PrescribedMedicines", prescribedMedicines));
            }
            else
            {
                aLocalReport.DataSources.Add(new ReportDataSource("PrescribedMedicines", new List<VMPrescribedMedicine>()));
            }
            if (data.Prescription != null)
            {
                var prescription = new List<Prescription>();
                prescription.Add(data.Prescription);
                aLocalReport.DataSources.Add(new ReportDataSource("Prescriptions", prescription));
            }
            else
            {
                aLocalReport.DataSources.Add(new ReportDataSource("Prescriptions", new List<Prescription>()));
            }
            if (data.MedicineHistories.Count > 0)
            {
                aLocalReport.DataSources.Add(new ReportDataSource("MedicineHistories", data.MedicineHistories));
            }
            else
            {
                aLocalReport.DataSources.Add(new ReportDataSource("MedicineHistories", new List<MedicineHistory>()));
            }

            aLocalReport.SubreportProcessing += (sender, e) => LocalReport_SubreportProcessing(sender, e, data);
            aLocalReport.Refresh();

            //PDF Output
            string mimeType;
            const string reportName = "Prescription";
            var renderedBytes = ReportUtility.RenderedReportViewer(aLocalReport, "PDF", out mimeType, reportName, isDownloadable: false);
            return await Task.Run(() => File(renderedBytes, mimeType));
        }

        private void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e, VMPreviewPrescription data)
        {
            var id = Convert.ToInt32(e.Parameters[0].Values[0]);
            switch (e.ReportPath)
            {
                case "CheifComplaientsSubReport":
                    var filterCC = data.CheifComplaients.FindAll(a => a.Id == id);
                    if (filterCC != null)
                    {
                        e.DataSources.Add(new ReportDataSource("CheifComplaientsSub", filterCC));
                    }
                    else
                    {
                        e.DataSources.Add(new ReportDataSource("CheifComplaientsSub", new List<ChiefComplaint>()));
                    }
                    break;
                case "IntraoralOnExaminationSubReport":
                    var filterOEI = data.OnExaminationIntraoral.FindAll(a => a.Id == id);
                    if (filterOEI != null)
                    {
                        e.DataSources.Add(new ReportDataSource("IntraoralOnExamination", filterOEI));
                    }
                    else
                    {
                        e.DataSources.Add(new ReportDataSource("IntraoralOnExamination", new List<OnExamination>()));
                    }
                    break;
                case "TreatmentPlansSubReport":
                    var filterTP = data.TreatmentPlans.FindAll(a => a.Id == id);
                    if (filterTP != null)
                    {
                        e.DataSources.Add(new ReportDataSource("TreatmentPlansSub", filterTP));
                    }
                    else
                    {
                        e.DataSources.Add(new ReportDataSource("TreatmentPlansSub", new List<TreatmentPlan>()));
                    }
                    break;
                default:
                    break;
            }

        }
    }
}