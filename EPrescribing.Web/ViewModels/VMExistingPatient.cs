using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPrescribing.Web.ViewModels
{
    public class VMExistingPatient
    {
        public int PatientIntId { get; set; }
        public string PatientID { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public string MobileNo { get; set; }
        public string Status { get; set; }
        public string Gender { get; internal set; }
        public DateTime CreatedDate { get; set; }
        public string LastSeenDate { get; set; }
        public DateTime TretmentDate { get; set; }
    }
}