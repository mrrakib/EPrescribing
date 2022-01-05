using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EPrescribing.Web.ViewModels
{
    public class VMChangeEmail
    {
        public int DoctorId { get; set; }
        [Required]
        public string CurrentEmail { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "New Email")]
        public string Email { get; set; }

        [Display(Name = "Confirm Email")]
        [Compare("Email", ErrorMessage = "The email and confirmation email do not match.")]
        public string ConfirmEmail { get; set; }
        public string EmailOtp { get; set; }
        public string Message { get; set; }
    }
}