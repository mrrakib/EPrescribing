using System;
using System.Security.Claims;
using System.Security.Principal;

namespace EPrescribing.Web.Helpers
{
    public static class IdentityData
    {
        public static string GETMOBILENO(this IPrincipal User)
        {
            var identity = (ClaimsIdentity)User.Identity;
            if (String.IsNullOrEmpty(identity.Name))
            {
                return "";
            }
            string context = identity.FindFirst("mobileno").Value;
            return context;
        }

        public static string GETROLENAME(this IPrincipal User)
        {
            var identity = (ClaimsIdentity)User.Identity;
            if (String.IsNullOrEmpty(identity.Name))
            {
                return "";
            }
            string context = identity.FindFirst("rolname").Value;
            return context;
        }

        public static string GETUSERNAME(this IPrincipal User)
        {
            var identity = (ClaimsIdentity)User.Identity;
            if (String.IsNullOrEmpty(identity.Name))
            {
                return "";
            }
            string context = identity.FindFirst("username").Value;
            return context;
        }
        public static string GETUSEREMAIL(this IPrincipal User)
        {
            var identity = (ClaimsIdentity)User.Identity;
            if (String.IsNullOrEmpty(identity.Name))
            {
                return "";
            }
            string context = identity.FindFirst("useremail").Value;
            return context;
        }
        public static string GETCLINICNAME(this IPrincipal User)
        {
            var identity = (ClaimsIdentity)User.Identity;
            if (String.IsNullOrEmpty(identity.Name))
            {
                return "";
            }
            string context = identity.FindFirst("clinicname").Value;
            return context;
        }

        public static string GETDOCTORIMAGE(this IPrincipal User)
        {
            var identity = (ClaimsIdentity)User.Identity;
            if (String.IsNullOrEmpty(identity.Name))
            {
                return "";
            }
            string context = identity.FindFirst("doctorImage")!=null? identity.FindFirst("doctorImage").Value:"";
            return context;
        }

        public static int GETDOCTORID(this IPrincipal User)
        {
            var identity = (ClaimsIdentity)User.Identity;
            if (String.IsNullOrEmpty(identity.Name))
            {
                return 0;
            }
            var val = identity.FindFirst("doctorid") != null ? identity.FindFirst("doctorid").Value : "0";
            int context = Convert.ToInt32(val);
            return context;
        }
        public static bool GETISACTIVE(this IPrincipal User)
        {
            var identity = (ClaimsIdentity)User.Identity;
            if (String.IsNullOrEmpty(identity.Name))
            {
                return false;
            }
            bool context = Convert.ToBoolean(identity.FindFirst("isactive").Value);
            return context;
        }
    }
}