using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPrescribing.Web.Models
{
    [Table("Brands")]
    public class Brand : BaseEntity<int>
    {
        [Required]
        [DisplayName("Brand Name")]
        public string BrandName { get; set; }

        [DisplayName("Location")]
        public string Location { get; set; }
    }
}
