namespace EPrescribing.Web.ViewModels.PrintPrescription
{
    public class VMOnExamination
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string UpperRight { get; set; }
        public string UpperLeft { get; set; }
        public string BottomRight { get; set; }
        public string BottomLeft { get; set; }
        public bool IsShow { get; set; }
    }
}