using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPrescribing.Web.Models
{
    [Table("Subscriptions")]
    public class Subscription : BaseEntity<int>
    {
        public Subscription()
        {
            EvaluationPeriodInDay = 0;
        }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [DisplayName("Duration")]
        public int EvaluationPeriodInDay { get; set; }
        [Required]
        public decimal Cost { get; set; }

        [NotMapped]
        public bool IsChecked { get; set; }
    }
}