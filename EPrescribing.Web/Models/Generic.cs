using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPrescribing.Web.Models
{
    [Table("Generics")]
    public class Generic : BaseEntity<int>
    {
        [Required]
        [StringLength(500)]
        [DisplayName("Generic Name")]
        public string GenericName { get; set; }

        [DisplayName("Dose Amount")]
        public string DoseAmount { get; set; }
        [DisplayName("Formulation")]
        public string Formulation { get; set; }
    }
}