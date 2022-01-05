using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPrescribing.Web.Models
{
    [Table("DiagnosticTest")]
    public class DiagnosticTest : BaseEntity<int>
    {
        [Required]
        public int PrescriptionId { get; set; }
        [ForeignKey("PrescriptionId")]
        public virtual Prescription Prescription { get; set; }
        //public int DiagnosticId { get; set; }
        //[ForeignKey("DiagnosticId")]
        //public virtual Diagnostic Diagnostic { get; set; }

        
        public string DiagnosticName { get; set; }
        [NotMapped]
        public string CategoryName { get; set; }
        [NotMapped]
        public int CategoryId { get; set; }
        [NotMapped]
        public int No { get; set; }
    }
}