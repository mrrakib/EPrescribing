namespace EPrescribing.Web.ViewModels.PrintPrescription
{
    public class VMCheifComplaient
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool UpperRight { get; set; }
        public bool UpperLeft { get; set; }
        public bool BottomRight { get; set; }
        public bool BottomLeft { get; set; }
        public bool IsShow { get; set; }
    }
}