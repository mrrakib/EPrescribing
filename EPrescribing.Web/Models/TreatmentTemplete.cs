using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EPrescribing.Web.Models
{

    [Table("TreatmentTempletes")]
    public class TreatmentTemplete : BaseEntity<int>
    {
        public int DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }

        public string TreatmentName { get; set; }
        public string PrescribedMedicine { get; set; }
        public string Advice { get; set; }
    }
}