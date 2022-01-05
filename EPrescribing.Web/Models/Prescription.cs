using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPrescribing.Web.Models
{
    [Table("Prescriptions")]
    public class Prescription : BaseEntity<int>
    {
        public Prescription()
        {
            ChiefComplaints = new List<ChiefComplaint>();
            MedicineHistory = new List<MedicineHistory>();
            OnExaminations = new List<OnExamination>();
            DiagnosticTest = new List<DiagnosticTest>();
            DifferentialDiagnosis = new List<DifferentialDiagnosis>();
            TreatmentPlans = new List<TreatmentPlan>();
            TreatmentDones = new List<TreatmentDone>();
            PrescribedMedicines = new List<PrescribedMedicine>();
            PrescribedAdvices = new List<PrescribedAdvice>();
        }

        [Required]
        public int DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }
        public int PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
        [DisplayName("Present Illnes")]
        public string PresentIllnes { get; set; }
        [DisplayName("Medical History")]
        public string MedicalHistory { get; set; }
        [DisplayName("Next Appointment Date")]
        public DateTime? NextAppointmentDate { get; set; }
        public string Status { get; set; }//H-History,E-Examination,C-Complete

        public int? PaymentId { get; set; }
        [ForeignKey("PaymentId")]
        public virtual Payment Payment { get; set; }

        public int? PreviousPrescriptionId { get; set; }

        #region NotMapped Data
        [NotMapped]
        [DisplayName("Total Charge")]
        public double TotalCharge { get; set; }

        [NotMapped]
        [DisplayName("Total Paid")]
        public double TotalPaid { get; set; }

        [NotMapped]
        [DisplayName("Total Due")]
        public double TotalDue { get; set; }

        [NotMapped]
        [DisplayName("Treatment Charge")]
        public double TreatmentCharge { get; set; }

        [NotMapped]
        [DisplayName("Today's Paid")]
        public double TodayPaid { get; set; }

        [NotMapped]
        [DisplayName("Today's Due")]
        public double TodayDue { get; set; }

        [NotMapped]
        public double TotalChargeBeforeThisPrescriptionPayment { get; set; }
        [NotMapped]
        public double TotalPaidBeforeThisPrescriptionPayment { get; set; }
        [NotMapped]
        public double TotalDueBeforeThisPrescriptionPayment { get; set; }

        public string MedicinePrescribed { get; set; }
        public string AdvicePrescribed { get; set; }
        [NotMapped]
        public string TrementTempleteName { get; set; }
        #endregion

        public virtual List<ChiefComplaint> ChiefComplaints { get; set; }
        public virtual List<MedicineHistory> MedicineHistory { get; set; }
        public virtual List<OnExamination> OnExaminations { get; set; }
        public virtual List<DiagnosticTest> DiagnosticTest { get; set; }
        public virtual List<DifferentialDiagnosis> DifferentialDiagnosis { get; set; }
        public virtual List<TreatmentPlan> TreatmentPlans { get; set; }
        public virtual List<TreatmentDone> TreatmentDones { get; set; }
        public virtual List<PrescribedMedicine> PrescribedMedicines { get; set; }
        public virtual List<PrescribedAdvice> PrescribedAdvices { get; set; }

    }
}
