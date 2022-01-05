using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPrescribing.Web.Models
{
    [Table("Companies")]
    public class Company : BaseEntity<int>
    {
        [Required]
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }
        [DisplayName("Company Code")]
        public string CompanyCode { get; set; }
        [DisplayName("Operation Start Date")]
        public DateTime? OperationStartDate { get; set; }
        public int Amount { get; set; }

    }
}