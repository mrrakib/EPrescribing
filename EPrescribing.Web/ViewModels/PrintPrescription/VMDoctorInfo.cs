namespace EPrescribing.Web.ViewModels.PrintPrescription
{
    public class VMDoctorInfo
    {
        public string Name { get; set; }
        public string SpecilizedDegree { get; set; }
        public string Designation { get; set; }
        public string BMDCRegistrationNumber { get; set; }
        public string ClinicName { get; set; }
        public string Organization { get; internal set; }
        public string ClinicAddress { get; internal set; }
    }
}