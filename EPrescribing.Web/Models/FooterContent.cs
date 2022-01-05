using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EPrescribing.Web.Models
{
    [Table("FooterContent")]
    public class FooterContent : BaseEntity<int>
    {
        [MaxLength(100, ErrorMessage = "Maximum 100 character allowed.")]
        [Required(ErrorMessage = "Footer Text is required.")]
        public string FooterText { get; set; }
        [MaxLength(150, ErrorMessage = "Maximum 150 character allowed.")]
        public string Facebook { get; set; }
        [MaxLength(150, ErrorMessage = "Maximum 150 character allowed.")]
        public string Twitter { get; set; }
        [MaxLength(150, ErrorMessage = "Maximum 150 character allowed.")]
        public string Tumblr { get; set; }
        [MaxLength(150, ErrorMessage = "Maximum 150 character allowed.")]
        public string Vimeo { get; set; }
    }
}