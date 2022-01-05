using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EPrescribing.Web.ViewModels
{
    public class VMChangeMobileNo
    {
        public int DoctorId { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Number")]
        public string CurrentMobileNo { get; set; }
        [Required]
        [Display(Name = "New Number")]
        public string MobileNo { get; set; }

        [Display(Name = "Confirm Number")]
        [Compare("MobileNo", ErrorMessage = "The new number and confirmation number do not match.")]
        public string ConfirmMobileNo { get; set; }
        public string MobileNoOtp { get; set; }
    }
}