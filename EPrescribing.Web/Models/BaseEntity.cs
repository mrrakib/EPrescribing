using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPrescribing.Web.Models
{
    public abstract class BaseEntity<T>
    {
        public BaseEntity()
        {
            CreatedDate = DateTime.Now;
            IsActive = true;
        }
        [Key]
        public T Id { get; set; }

        [DisplayName("Created Date")]
        //[DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Updated Date")]
        //[DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime? UpdatedDate { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }

        [NotMapped]
        [DisplayName("Is Active")]
        public string IsActiveDisplay { get { return IsActive ? "Yes" : "No"; } }
    }
}