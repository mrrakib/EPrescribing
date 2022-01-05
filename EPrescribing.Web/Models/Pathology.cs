using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPrescribing.Web.Models
{
    [Table("Pathologies")]
    public class Pathology : BaseEntity<int>
    {
        [Required]
        [StringLength(500)]
        [DisplayName("Pathology Name")]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }
    }
}

