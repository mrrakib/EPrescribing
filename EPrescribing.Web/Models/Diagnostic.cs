using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPrescribing.Web.Models
{
    [Table("Diagnostics")]
    public class Diagnostic : BaseEntity<int>
    {
        [DisplayName("Parent")]
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual Diagnostic Parent { get; set; }
        [Required]
        [StringLength(500)]
        [DisplayName("Test Name")]
        public string TestName { get; set; }
        [StringLength(1000)]
        public string Note { get; set; }
    }
}