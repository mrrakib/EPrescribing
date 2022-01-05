using System.ComponentModel.DataAnnotations.Schema;

namespace EPrescribing.Web.Models
{
    [Table("ChiefComplaints")]
    public class ChiefComplaint : BaseEntity<int>
    {
        public string Description { get; set; }
        public bool UpperRight { get; set; }
        public bool UpperLeft { get; set; }
        public bool BottomRight { get; set; }
        public bool BottomLeft { get; set; }

        public int PrescriptionId { get; set; }
        [ForeignKey("PrescriptionId")]
        public virtual Prescription Prescription { get; set; }

        [NotMapped]
        public int No { get; set; }
    }
}