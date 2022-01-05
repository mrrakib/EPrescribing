using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EPrescribing.Web.Models
{
    public class SingleServiceSection : BaseEntity<int>
    {
        [MaxLength(100, ErrorMessage = "Maximum 100 character allowed.")]
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }
        [MaxLength(25, ErrorMessage = "Maximum 25 character allowed.")]
        public string Icon { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(150, ErrorMessage = "Maximum 150 character allowed.")]
        public string Description { get; set; }
    }
}