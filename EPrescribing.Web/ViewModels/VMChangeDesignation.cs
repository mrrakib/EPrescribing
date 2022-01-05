using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EPrescribing.Web.ViewModels
{
    public class VMChangeDesignation
    {
        public int DoctorId { get; set; }
        [Required]
        [Display(Name = "Designation")]
        public int DesignationId { get; set; }
        [Display(Name = "BMDC Reg No")]
        public string BMDCRegistrationNumber { get; set; }

    }
}