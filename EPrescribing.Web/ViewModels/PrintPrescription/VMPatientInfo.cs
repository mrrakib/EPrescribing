using System;
using System.ComponentModel;

namespace EPrescribing.Web.ViewModels.PrintPrescription
{
    public class VMPatientInfo
    {
        public string PatientID { get; set; }
        public string Name { get; set; }
        [DisplayName("Sex")]
        public string Gender { get; set; }
        public string Age { get; set; }
        [DisplayName("Mobile No")]
        public string MobileNo { get; set; }

        public DateTime TretmentDate { get; set; }
    }
}