using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPrescribing.Web.Models
{
    [Table("Doctors")]
    public class Doctor : BaseEntity<int>
    {
        //public int UserId { get; set; }
        //[ForeignKey("UserId")]
        //public virtual LoginUser User { set; get; }

        public string AppUserID { get; set; }
        [ForeignKey("AppUserID")]
        public virtual ApplicationUser AppUser { set; get; }
        public string Name { get; set; }
        public string Image { get; set; }
        [DisplayName("BMDC Reg")]
        public string BMDCRegistrationNumber { get; set; }
        public int? DesignationId { get; set; }
        [ForeignKey("DesignationId")]
        public virtual Designation Designation { set; get; }
        public int? DentalSchoolId { get; set; }
        [ForeignKey("DentalSchoolId")]
        public virtual DentalSchool DentalSchool { set; get; }
        public string ClinicName { get; set; }
        public string ClinicAddress { get; set; }
        public DateTime? SubscribedDate { get; set; }
        public int? SubscriptionlId { get; set; }
        [ForeignKey("SubscriptionlId")]
        public virtual Subscription Subscription { set; get; }
        public bool IsBMDCVerified { get; set; }
        [NotMapped]
        public string Email { get; set; }
        [NotMapped]
        public string Password { get; set; }
        [NotMapped]
        public string ConfirmPassword { get; set; }
        [NotMapped]
        public string MobileNo { get; set; }
        [NotMapped]
        public virtual List<Subscription> Subscriptions { get; set; }
        public DateTime? SubscriptionExpiredDate { get; internal set; }
    }
}