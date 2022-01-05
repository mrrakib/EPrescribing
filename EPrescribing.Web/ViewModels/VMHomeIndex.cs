using EPrescribing.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPrescribing.Web.ViewModels
{
    public class VMHomeIndex
    {
        public AboutSection AboutSection { get; set; }
        public WorkProcess WorkProcess { get; set; }
        public ServiceMainSection ServiceMain { get; set; }
        public List<SingleServiceSection> ServiceList { get; set; }
        public TeamMainSection TeamMainSection { get; set; }
        public List<TeamMember> TeamMembers { get; set; }
        public List<ContactSection> ContactSections { get; set; }
        public VMStatistics statistics { get; set; }
    }
}