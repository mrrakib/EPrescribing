using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPrescribing.Web.Models
{
    [Table("Diseases")]
    public class Disease : BaseEntity<int>
    {
        [Required]
        [StringLength(500)]
        [DisplayName("Disease Name")]
        public string Name { get; set; }
    }
}