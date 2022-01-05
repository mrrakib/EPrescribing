using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPrescribing.Web.Models
{
    [Table("MedicineHistories")]
    public class MedicineHistory : BaseEntity<int>
    {
        //[Required]
        //public int MedicineId { get; set; }
        //[ForeignKey("MedicineId")]
        //public virtual Medicine Medicine { get; set; }

        public string Dose { get; set; }
        public string Duration { get; set; }
        public string Note { get; set; }
        public string MedicineName { get; set; }

        public int PrescriptionId { get; set; }
        [ForeignKey("PrescriptionId")]
        public virtual Prescription Prescription { get; set; }
        [NotMapped]
        public int GenericId { get; set; }
        [NotMapped]
        public string GenericName { get; set; }
        [NotMapped]
        public int No { get; set; }
    }
}