using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPrescribing.Web.ViewModels
{
    public class VMPatientRecord
    {
        public int PatientId { get; set; }
        public string PatientStringID { get; set; }
        public int PrescriptionId { get; set; }
        public string PatientName { get; set; }
        public string Age { get; set; }
        public string Status { get; set; }
        public string MobileNo { get; set; }
        public DateTime Date { get; set; }
        public string NextVisitDate { get; set; }
        public string Gender { get; internal set; }
    }
}