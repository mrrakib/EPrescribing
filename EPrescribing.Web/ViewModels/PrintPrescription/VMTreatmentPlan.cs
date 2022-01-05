using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPrescribing.Web.ViewModels.PrintPrescription
{
    public class VMTreatmentPlan
    {
        public int Id { get; set; }
        public string TreatmentName { get; set; }
        public string UpperRight { get; set; }
        public string UpperLeft { get; set; }
        public string BottomRight { get; set; }
        public string BottomLeft { get; set; }
        public bool IsShow { get; set; }
    }
}