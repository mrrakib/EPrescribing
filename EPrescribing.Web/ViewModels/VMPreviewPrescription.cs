using EPrescribing.Web.Models;
using System.Collections.Generic;

namespace EPrescribing.Web.ViewModels
{
    public class VMPreviewPrescription
    {
        public Patient PatientInfo { get; set; }
        public List<ChiefComplaint> CheifComplaients { get; internal set; }
        public Prescription Prescription { get; internal set; }
        public List<MedicineHistory> MedicineHistories { get; internal set; }
        public List<OnExamination> OnExaminationExtraoral { get; internal set; }
        public List<OnExamination> OnExaminationIntraoral { get; internal set; }
        public List<TreatmentPlan> TreatmentPlans { get; internal set; }
        public List<DifferentialDiagnosis> DifferentialDiagnosis { get; internal set; }
        public List<DiagnosticTest> DiagnosticTests { get; internal set; }
        public List<TreatmentDone> TreatmentDones { get; internal set; }
        public List<PrescribedMedicine> PrescribedMedicines { get; internal set; }
        public List<PrescribedAdvice> PrescribedAdvices { get; internal set; }
        public int PrescriptionId { get; internal set; }
        public Doctor DoctorInfo { get; internal set; }
    }
}