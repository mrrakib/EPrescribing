using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPrescribing.Web.Models
{
    [Table("TreatmentPlans")]
    public class TreatmentPlan : BaseEntity<int>
    {
        [DisplayName("Treatment Name")]
        public string TreatmentName { get; set; }

        public string UpperRight { get; set; }
        public string UpperLeft { get; set; }
        public string BottomRight { get; set; }
        public string BottomLeft { get; set; }

        public int PrescriptionId { get; set; }
        [ForeignKey("PrescriptionId")]
        public virtual Prescription Prescription { get; set; }
        [NotMapped]
        public int No { get; set; }
    }
}
