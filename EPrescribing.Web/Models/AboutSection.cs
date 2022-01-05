using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EPrescribing.Web.Models
{
    public class AboutSection : BaseEntity<int>
    {
        [MaxLength(100, ErrorMessage = "Maximum 100 character allowed.")]
        [Required(ErrorMessage = "Top title is required.")]
        public string TopTitle { get; set; }
        [Required(ErrorMessage = "Main title is required.")]
        public string MainTitle { get; set; }
        [Required(ErrorMessage = "Description title is required.")]
        public string Description { get; set; }
        [MaxLength(100, ErrorMessage = "Maximum 100 character allowed.")]
        [Required(ErrorMessage = "First list title is required.")]
        public string ListOneTitle { get; set; }
        [Required(ErrorMessage = "First list description is required.")]
        public string ListOneDescription { get; set; }
        [Required(ErrorMessage = "First list icon is required.")]
        [MaxLength(50, ErrorMessage = "Maximum 50 character allowed.")]
        public string ListOneIcon { get; set; }
        [MaxLength(100, ErrorMessage = "Maximum 100 character allowed.")]
        [Required(ErrorMessage = "Second list title is required.")]
        public string ListSecondTitle { get; set; }
        [Required(ErrorMessage = "Second list description is required.")]
        public string ListSecondDescription { get; set; }
        [Required(ErrorMessage = "Second list icon is required.")]
        [MaxLength(50, ErrorMessage = "Maximum 50 character allowed.")]
        public string ListSecondIcon { get; set; }
        public string ImagePath { get; set; }
    }
}