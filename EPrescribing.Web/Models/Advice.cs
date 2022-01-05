using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPrescribing.Web.Models
{
    [Table("Advices")]
    public class Advice : BaseEntity<int>
    {
        [Required]
        [DisplayName("Advice Name")]
        public string AdviceName { get; set; }
    }
}