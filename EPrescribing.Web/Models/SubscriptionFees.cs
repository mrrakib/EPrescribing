using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPrescribing.Web.Models
{
    public class SubscriptionFees : BaseEntity<int>
    {
        public int DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }
        public int SubscriptionId { get; set; }
        [ForeignKey("SubscriptionId")]
        public virtual Subscription Subscription { get; set; }
        [Required]
        [DisplayName("Payment Method")]
        public string PaymentMethod { get; set; }
        [Required]
        [DisplayName("Transaction ID")]
        public string TransactionNo { get; set; }
        public string Reference { get; set; }
        [Required]
        [DisplayName("Account No")]
        public string DebitAccount { get; set; }
        [DisplayName("Payment To")]
        public string CreditAccount { get; set; }
        [Required]
        [DisplayName("Amount")]
        public decimal PayableAmount { get; set; }
    }
}
