using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPrescribing.Web.Models
{
    [Table("Medicines")]
    public class Medicine : BaseEntity<int>
    {
        [Required]
        [StringLength(500)]
        [DisplayName("Brand Name")]
        public string Name { get; set; }
       
        [DisplayName("Dose Amount")]
        public string DoseAmount { get; set; }
        [DisplayName("Formulation")]
        public string Formulation { get; set; }

        public int? GenericId { get; set; }
        [ForeignKey("GenericId")]
        public virtual Generic Generic { get; set; }

        public int? BrandId { get; set; }
        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }

    }
}