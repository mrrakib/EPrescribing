using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EPrescribing.Web.Models
{
    public class ContactSection : BaseEntity<int>
    {
        public string Title { get; set; }
        [MaxLength(300, ErrorMessage = "Maximum 300 character allowed.")]
        public string Address { get; set; }
        [MaxLength(50, ErrorMessage = "Maximum 50 character allowed.")]
        [Required(ErrorMessage = "Location icon is required.")]
        public string Icon { get; set; }
        [MaxLength(20, ErrorMessage = "Maximum 20 character allowed.")]
        public string PhoneNo1 { get; set; }
        [MaxLength(20, ErrorMessage = "Maximum 20 character allowed.")]
        public string PhoneNo2 { get; set; }
        [MaxLength(80, ErrorMessage = "Maximum 80 character allowed.")]
        public string Email1 { get; set; }
        [MaxLength(80, ErrorMessage = "Maximum 80 character allowed.")]
        public string Email2 { get; set; }
    }
}