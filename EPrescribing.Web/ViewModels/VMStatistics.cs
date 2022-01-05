using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPrescribing.Web.ViewModels
{
    public class VMStatistics
    {
        public double TotalPaidAmount { get; set; }
        public double TotalDueAmount { get; set; }
        public double TodayTotalPaidAmount { get; set; }
        public double TodayTotalDueAmount { get; set; }
        public double CurrentMonthPaid { get; set; }
        public double CurrentMonthDue { get; set; }
        public string YearlyPaid { get; set; }
        public string YearlyDue { get; set; }
    }
}