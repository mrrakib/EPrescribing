using EPrescribing.Web.Data;
using EPrescribing.Web.Data.Services;
using EPrescribing.Web.Helpers;
using EPrescribing.Web.Models;
using EPrescribing.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace EPrescribing.Web.Controllers
{
    public class HomeController : Controller
    {

        private AppEntities db = new AppEntities();
        private readonly IAboutSectionService _aboutSectionService;
        private readonly IWorkProcessService _workProcessService;
        private readonly IServiceMainSectionService _serviceMainSectionService;
        private readonly ISingleServiceSectionService _singleServiceSectionService;
        private readonly IFooterContentService _footerContentService;
        private readonly ITeamMainSectionService _teamMainSectionService;
        private readonly ITeamMemberService _teamMemberService;
        private readonly IContactSectionService _contactSectionService;
        private readonly IDoctorService _doctorService;
        Message _message = new Message();
        public HomeController()
        {
            _aboutSectionService = new AboutSectionService(db);
            _workProcessService = new WorkProcessService(db);
            _serviceMainSectionService = new ServiceMainSectionService(db);
            _singleServiceSectionService = new SingleServiceSectionService(db);
            _footerContentService = new FooterContentService(db);
            _teamMainSectionService = new TeamMainSectionService(db);
            _teamMemberService = new TeamMemberService(db);
            _contactSectionService = new ContactSectionService(db);
            _doctorService = new DoctorService(db);
        }

        public ActionResult Index()
        {
            VMHomeIndex index = new VMHomeIndex();
            AboutSection about = _aboutSectionService.GetTopOne();
            WorkProcess workProcess = _workProcessService.GetTopOne();
            ServiceMainSection serviceMain = _serviceMainSectionService.GetTopOne();
            List<SingleServiceSection> serviceList = _singleServiceSectionService.GetAll().ToList();
            TeamMainSection teamMain = _teamMainSectionService.GetTopOne();
            List<TeamMember> teamMembers = _teamMemberService.GetAll().ToList();
            List<ContactSection> contactSection = _contactSectionService.GetAll().ToList();
            VMStatistics statistics = _doctorService.GetStatistics(User.GETDOCTORID());
            

            index.AboutSection = about;
            index.WorkProcess = workProcess;
            index.ServiceMain = serviceMain;
            index.ServiceList = serviceList;
            index.TeamMainSection = teamMain;
            index.TeamMembers = teamMembers;
            index.ContactSections = contactSection;
            index.statistics = statistics;

            return View(index);
        }

        public ActionResult GetFooter()
        {
            FooterContent footerContent = _footerContentService.GetTopOne();
            return PartialView("~/Views/Shared/_footer.cshtml", footerContent);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpGet]
        public ActionResult UnAuthorized()
        {
            return View();
        }

        
    }
}