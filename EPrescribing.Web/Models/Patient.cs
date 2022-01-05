using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPrescribing.Web.Models
{
    [Table("Patients")]
    public class Patient : BaseEntity<int>
    {
        public Patient()
        {
            Prescriptions = new List<Prescription>();
            TotalDiscount = 0;
        }

        [StringLength(14)]
        public string PatientID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DisplayName("Sex")]
        public string Gender { get; set; }
        [Required]
        public string Age { get; set; }
        [Required]
        [DisplayName("Mobile No")]
        public string MobileNo { get; set; }

        public DateTime TretmentDate { get; set; }
        public DateTime? NextAppointmentDate { get; set; }

        public virtual List<Prescription> Prescriptions { get; set; }

        [Required]
        public int DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }

        [DisplayName("Total Cost")]
        public double TotalCharge { get; set; }
        [DisplayName("Total Paid")]
        public double TotalPaid { get; set; }

        [DisplayName("Total Due")]
        public double TotalDue { get; set; }
        public double TotalDiscount { get; set; }

        [NotMapped]
        public string Status { get; set; }
    }
}
