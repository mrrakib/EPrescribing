using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPrescribing.Web.Models
{
    [Table("Treatments")]
    public class Treatment : BaseEntity<int>
    {
        [Required]
        [StringLength(500)]
        [DisplayName("Treatment Name")]
        public string Name { get; set; }
    }
}
