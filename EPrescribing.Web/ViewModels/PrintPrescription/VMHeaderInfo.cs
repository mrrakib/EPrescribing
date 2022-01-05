namespace EPrescribing.Web.ViewModels.PrintPrescription
{
    public class VMHeaderInfo
    {
        public string Name { get; set; }
        public string OrganizationName { get; set; }
        public string Location { get; set; }
        public string Logo { get; set; }
        public string RxLogo { get; internal set; }
        public string WatermarkLogo { get; internal set; }
    }
}