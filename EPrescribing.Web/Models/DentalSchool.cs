using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPrescribing.Web.Models
{
    [Table("DentalSchools")]
    public class DentalSchool : BaseEntity<int>
    {
        [Required]
        public string Name { get; set; }
        public string Code { get; set; }
    }
}