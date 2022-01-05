using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPrescribing.Web.Models
{
    [Table("PrescribedMedicine")]
    public class PrescribedMedicine : BaseEntity<int>
    {
        //[Required]
        //public int MedicineId { get; set; }
        //[ForeignKey("MedicineId")]
        //public virtual Medicine Medicine { get; set; }
        [DisplayName("Daily Dose")]
        public string DailyDose { get; set; }
        public string Days { get; set; }
        public string Notes { get; set; }

        public int PrescriptionId { get; set; }
        [ForeignKey("PrescriptionId")]
        public virtual Prescription Prescription { get; set; }
        public string MedicineName { get; set; }
        [NotMapped]
        public int GenericId { get; set; }
        [NotMapped]
        public string GenericName { get; set; }
        [NotMapped]
        public int No { get; set; }
    }
}