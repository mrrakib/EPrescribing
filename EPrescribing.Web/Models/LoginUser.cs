using System.ComponentModel.DataAnnotations.Schema;

namespace EPrescribing.Web.Models
{
    [Table("LoginUsers")]
    public class LoginUser : BaseEntity<int>
    {
        public string UserName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Otp { get; set; }
        public string OtpKey { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        public string EmailOtp { get; set; }
    }
}