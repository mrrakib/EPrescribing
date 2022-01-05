using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPrescribing.Web.Models
{
    [Table("Payments")]
    public class Payment : BaseEntity<int>
    {
        public Payment()
        {
            TotalDiscount = 0;
        }
        public int PatientId { get; set; }
        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        [DisplayName("Treatment Charge")]
        public double TreatmentCharge { get; set; }
        [DisplayName("Paid Amount")]
        public double PaidAmount { get; set; }
        [DisplayName("Due Amount")]
        public double DueAmount { get; set; }
        [DisplayName("Total Discount")]
        public double TotalDiscount { get; set; }

        [DisplayName("Payment Date")]
        public DateTime PaymentDate { get; set; }

        [Required]
        public int PrescriptionId { get; set; }
        public double TotalDueAmount { get; internal set; }
    }
}
