using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPrescribing.Web.Models
{
    [Table("DifferentialDiagnosis")]
    public class DifferentialDiagnosis : BaseEntity<int>
    {
        [DisplayName("Disease Name")]
        public string DiseaseName { get; set; }

        [Required]
        public int PrescriptionId { get; set; }
        [ForeignKey("PrescriptionId")]
        public virtual Prescription Prescription { get; set; }
        [NotMapped]
        public int No { get; set; }
    }
}