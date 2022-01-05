using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPrescribing.Web.ViewModels
{
    public class VMPatient
    {
        public int Id { get; set; }
        public string PatientId { get; set; }
        public string MobileNo { get; set; }
        public string Status { get; set; }
        public DateTime TretmentDate { get; set; }
    }
}