using System.Web;

namespace EPrescribing.Web.Helpers
{
    public class AppSession
    {
        private const string roleId = "RoleId";
        private const string roleName = "RoleName";
        private const string userName = "UserName";
        private const string mobileNo = "MobileNo";

        private const string userId = "UserId";
        private const string doctorId = "DoctorId";
        private const string isActive = "IsActive";

        public static string RoleId
        {
            get
            {
                return (string)(HttpContext.Current.Session[roleId] ?? "");
            }
            set
            {
                HttpContext.Current.Session[roleId] = value;
            }
        }

        public static string RoleName
        {
            get
            {
                return (string)(HttpContext.Current.Session[roleName] ?? "");
            }
            set
            {
                HttpContext.Current.Session[roleName] = value;
            }
        }
        public static string UserName
        {
            get
            {
                return (string)(HttpContext.Current.Session[userName] ?? "");
            }
            set
            {
                HttpContext.Current.Session[userName] = value;
            }
        }
        public static string MobileNo
        {
            get
            {
                return (string)(HttpContext.Current.Session[mobileNo] ?? "");
            }
            set
            {
                HttpContext.Current.Session[mobileNo] = value;
            }
        }
        public static int UserId
        {
            get
            {
                if (HttpContext.Current.Session[userId] != null)
                    return (int)HttpContext.Current.Session[userId];
                else
                    return 0;
            }
            set
            {
                HttpContext.Current.Session[userId] = value;
            }
        }
        public static int DoctorId
        {
            get
            {
                if (HttpContext.Current.Session[doctorId] != null)
                    return (int)HttpContext.Current.Session[doctorId];
                else
                    return 0;
            }
            set
            {
                HttpContext.Current.Session[doctorId] = value;
            }
        }
        public static bool IsActive
        {
            get
            {
                if (HttpContext.Current.Session[isActive] != null)
                    return (bool)HttpContext.Current.Session[isActive];
                return false;
            }
            set
            {
                HttpContext.Current.Session[isActive] = value;
            }
        }
        public static void Clear()
        {
            HttpContext.Current.Session.Clear();

        }
    }
}