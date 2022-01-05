using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EPrescribing.Web.ViewModels
{
    public class VMLoginUser
    {
        [Required]
        [RegularExpression(@"(^([+]{1}[8]{2}|0088)?(01){1}[3-9]{1}\d{8})$", ErrorMessage ="Please provide a valid mobile no.")]
        [Display(Name = "Mobile No")]
        public string MobileNo { get; set; }
        [Required]
        [Display(Name = "OTP")]
        public string Otp { get; set; }
        public bool Status { get; set; }
        public string ReturnUrl { get; set; }
    }
}