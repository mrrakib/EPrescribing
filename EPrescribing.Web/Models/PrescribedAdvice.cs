using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPrescribing.Web.Models
{
    [Table("PrescribedAdvices")]
    public class PrescribedAdvice : BaseEntity<int>
    {
        [DisplayName("Advice Name")]
        public string AdviceName { get; set; }

        public int PrescriptionId { get; set; }
        [ForeignKey("PrescriptionId")]
        public virtual Prescription Prescription { get; set; }
        [NotMapped]
        public int No { get; set; }
    }
}