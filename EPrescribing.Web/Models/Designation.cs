using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPrescribing.Web.Models
{
    [Table("Designations")]
    public class Designation : BaseEntity<int>
    {
        [Required]
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsSubscriptionFree { get; set; }
    }
}