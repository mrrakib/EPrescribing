using EPrescribing.Web.Data;
using EPrescribing.Web.Data.Services;
using EPrescribing.Web.Helpers;
using EPrescribing.Web.Models;
using EPrescribing.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using OtpNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EPrescribing.Web.Controllers
{
    public class AccountController : Controller
    {
        private AppEntities db = new AppEntities();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly OtpConfiguration _otpConfig;
        private IDoctorService _doctorService;
        private IUserService _userService;
        private IDesignationService _designationService;
        private IDentalSchoolService _dentalSchoolService;
        private ISubscriptionService _subscriptionService;
        Message _message = new Message();
        public AccountController() : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new AppEntities())))
        {
            _otpConfig = new OtpConfiguration(200, 6);
            _doctorService = new DoctorService(db);
            _userService = new UserService(db);
            _designationService = new DesignationService(db);
            _dentalSchoolService = new DentalSchoolService(db);
            _subscriptionService = new SubscriptionService(db);
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }
        public UserManager<ApplicationUser> UserManager { get; private set; }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [AllowAnonymous]
        public ActionResult EmailLogin(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(VMLoginUser model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (ModelState.ContainsKey("Otp"))
                ModelState["Otp"].Errors.Clear();

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            ApplicationUser identityUser = null;
            var key = KeyGeneration.GenerateRandomKey(10);
            string randkey = Convert.ToBase64String(key);
            var totp = new Totp(key, step: _otpConfig.Time, totpSize: _otpConfig.Length, timeCorrection: null);
            var otp = totp.ComputeTotp(DateTime.UtcNow);

            identityUser = await UserManager.FindByNameAsync(model.MobileNo);

            IdentityResult result = new IdentityResult();


            if (identityUser != null)
            {
                identityUser.Otp = otp;
                identityUser.OtpKey = randkey;

                result = await UserManager.UpdateAsync(identityUser);
            }
            else
            {
                identityUser = new ApplicationUser();
                identityUser.UserName = model.MobileNo;
                identityUser.Otp = otp;
                identityUser.OtpKey = randkey;
                identityUser.RoleName = "General User";

                UserManager.PasswordValidator = new CustomPasswordValidator();
                UserManager.PasswordHasher = new CustomPasswordHasher();
                result = await UserManager.CreateAsync(identityUser);
            }
            if (result.Succeeded)
            {


                var doctor = _doctorService.GetDoctorByAppUserID(identityUser.Id);
                if (doctor == null)
                {
                    Doctor newdoctor = new Doctor();
                    newdoctor.AppUserID = identityUser.Id;
                    newdoctor.IsActive = false;

                    _doctorService.Add(newdoctor);
                }
                string message = "Use " + otp + " as your OTP to login to ODC Soft. Thank you.";
                var SmsSendStatus = SMSGateway.SendSMS(message, model.MobileNo);
                if (SmsSendStatus.status_code == 200)
                {
                    _message.success(this, "An OTP Send to your mobile number");
                    return View("Otp", model);
                }
                else
                {
                    ModelState.AddModelError("", SmsSendStatus.error_message);
                    return View(model);
                }

            }
            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> ResendOtp(string mobileNo)
        {
            ApplicationUser identityUser = null;
            var key = KeyGeneration.GenerateRandomKey(10);
            string randkey = Convert.ToBase64String(key);
            var totp = new Totp(key, step: _otpConfig.Time, totpSize: _otpConfig.Length, timeCorrection: null);
            var otp = totp.ComputeTotp(DateTime.UtcNow);

            identityUser = await UserManager.FindByNameAsync(mobileNo);

            IdentityResult result = null;

            if (identityUser != null)
            {
                identityUser.Otp = otp;
                identityUser.OtpKey = randkey;

                result = await UserManager.UpdateAsync(identityUser);
            }
            else
            {
                identityUser = new ApplicationUser();
                identityUser.UserName = mobileNo;
                identityUser.Otp = otp;
                identityUser.OtpKey = randkey;
                identityUser.RoleName = "General User";

                UserManager.PasswordValidator = new CustomPasswordValidator();
                UserManager.PasswordHasher = new CustomPasswordHasher();

                result = await UserManager.CreateAsync(identityUser);
            }

            if (result.Succeeded)
            {
                var doctor = _doctorService.GetDoctorByAppUserID(identityUser.Id);
                if (doctor == null)
                {
                    Doctor newdoctor = new Doctor();
                    newdoctor.AppUserID = identityUser.Id;
                    newdoctor.IsActive = false;

                    _doctorService.Add(newdoctor);
                }
                string message = "Use " + otp + " as your OTP to login to ODC Soft. Thank you.";
                var res = SMSGateway.SendSMS(message, mobileNo);
                if (res.status_code == 200)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(false, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EmailLogin(VMEmailLogin model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            ApplicationUser identityUser = null;

            identityUser = await UserManager.FindByEmailAsync(model.Email);
            if (identityUser != null)
            {
                var userWithPassword = await UserManager.FindAsync(identityUser.UserName, model.Password);
                if (userWithPassword != null)
                {
                    var doctor = _doctorService.GetDoctorByAppUserID(userWithPassword.Id);
                    await SignInAsync(identityUser, doctor, true);
                    _message.success(this, "Login Successfully");
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    //_message.custom(this, "Login Failed");
                    ModelState.AddModelError("", "Invalid Password");
                    return View(model);
                }
            }
            else
            {
                //_message.custom(this, "Login Failed");
                ModelState.AddModelError("", "Invalid Email");
                return View(model);
            }

        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Otp(VMLoginUser model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ActionName("Otp")]
        public async Task<ActionResult> OtpPOST(VMLoginUser model, string returnUrl)
        {
            if (string.IsNullOrEmpty(model.Otp))
            {
                ModelState.AddModelError("Otp", "Otp is required");
            }
            if (ModelState.IsValid)
            {
                model.Otp = model.Otp.Replace(" ", "");
                ApplicationUser identityUser = null;
                identityUser = await UserManager.FindByNameAsync(model.MobileNo);
                Doctor doctor = null;
                if (identityUser != null)
                {
                    bool isOTPWrong = identityUser.Otp.Equals(model.Otp);
                    if (isOTPWrong)
                    {
                        doctor = _doctorService.GetDoctorByAppUserID(identityUser.Id);

                        byte[] Bytekey = Convert.FromBase64String(identityUser.OtpKey);
                        var totp = new Totp(Bytekey, step: _otpConfig.Time, totpSize: _otpConfig.Length, timeCorrection: null);
                        long timeStepMatched;
                        bool verify = totp.VerifyTotp(DateTime.UtcNow, model.Otp, out timeStepMatched, VerificationWindow.RfcSpecifiedNetworkDelay);

                        if (verify)
                        {
                            identityUser.Otp = "";
                            identityUser.OtpKey = "";
                            await UserManager.UpdateAsync(identityUser);
                            await SignInAsync(identityUser, doctor, true);
                            _message.success(this, "Login Successfully");
                            return RedirectToLocal(returnUrl);
                        }
                        else
                        {
                            ModelState.AddModelError("", "OTP timeout");
                            return View("Otp", model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "OTP is incorrect");
                        return View("Otp", model);
                    }
                }
            }
            return View("Otp", model);
        }

        internal class CustomPasswordValidator : PasswordValidator
        {
            public override async Task<IdentityResult> ValidateAsync(string item)
            {
                if (string.IsNullOrEmpty(item)) return IdentityResult.Success;
                return await base.ValidateAsync(item);
            }
        }

        internal class CustomPasswordHasher : PasswordHasher
        {
            public override PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
            {
                if (hashedPassword == null && string.IsNullOrEmpty(providedPassword))
                    return PasswordVerificationResult.Success;
                return base.VerifyHashedPassword(hashedPassword, providedPassword);
            }
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        private async Task SignInAsync(ApplicationUser identityUser, Doctor doctor, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            var identity = await UserManager.CreateIdentityAsync(identityUser, DefaultAuthenticationTypes.ApplicationCookie);

            identity.AddClaim(new Claim("mobileno", identityUser.UserName.ToString()));
            identity.AddClaim(new Claim("rolname", identityUser.RoleName.ToString()));
            identity.AddClaim(new Claim("username", string.IsNullOrEmpty(doctor.Name) ? "" : doctor.Name.ToString()));
            identity.AddClaim(new Claim("useremail", string.IsNullOrEmpty(identityUser.Email) ? "" : identityUser.Email.ToString()));
            identity.AddClaim(new Claim("clinicname", string.IsNullOrEmpty(doctor.ClinicName) ? "" : doctor.ClinicName.ToString()));
            identity.AddClaim(new Claim("doctorImage", string.IsNullOrEmpty(doctor.Image) ? "" : doctor.Image.ToString()));
            identity.AddClaim(new Claim("doctorid", doctor.Id.ToString()));
            identity.AddClaim(new Claim("isactive", doctor.IsActive.ToString()));
           
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        [AppAuthorization]
        public async Task<ActionResult> UpdateAccount()
        {
            var identityUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            Doctor doctor = null;
            doctor = _doctorService.GetDoctorByAppUserID(identityUser.Id);
            if (doctor != null)
            {
                doctor.MobileNo = identityUser.UserName;
                doctor.Email = identityUser.Email;
            }
            else
            {
                doctor = new Doctor();
                doctor.MobileNo = identityUser.UserName;
            }
            doctor.Subscriptions = null;

            ViewBag.DesignationId = new SelectList(_designationService.GetAll(), "Id", "Name", doctor.DesignationId);
            ViewBag.DentalSchoolId = new SelectList(_dentalSchoolService.GetAll(), "Id", "Name", doctor.DentalSchoolId);
            return View(doctor);
        }

        [HttpPost]
        [AppAuthorization]
        public async Task<ActionResult> UpdateAccount(Doctor model, HttpPostedFileBase image)
        {
            ViewBag.DesignationId = new SelectList(_designationService.GetAll(), "Id", "Name", model.DesignationId);
            ViewBag.DentalSchoolId = new SelectList(_dentalSchoolService.GetAll(), "Id", "Name", model.DentalSchoolId);
            var identityUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            IdentityResult passwordChageResult = null;
            if (!string.IsNullOrEmpty(model.Password))
            {

                string newPasswordHash = UserManager.PasswordHasher.HashPassword(model.Password);
                identityUser.PasswordHash = newPasswordHash;
                passwordChageResult = await UserManager.UpdateAsync(identityUser);

            }
            if (passwordChageResult.Succeeded)
            {
                identityUser.Email = model.Email;
                UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager)
                {
                    RequireUniqueEmail = true
                };
                var updated = await UserManager.UpdateAsync(identityUser);
                if (updated.Succeeded)
                {
                    model.IsActive = false;
                    model.AppUserID = User.Identity.GetUserId();

                    var doctor = _doctorService.GetDoctorByAppUserID(User.Identity.GetUserId());
                    if (doctor != null)
                    {
                        if (image != null)
                        {
                            string ImageName = UploadImage(image, identityUser.UserName);
                            doctor.Image = ImageName;
                        }

                        doctor.Name = model.Name;
                        doctor.BMDCRegistrationNumber = model.BMDCRegistrationNumber;
                        doctor.ClinicName = model.ClinicName;
                        doctor.ClinicAddress = model.ClinicAddress;
                        doctor.DentalSchoolId = model.DentalSchoolId;
                        doctor.DesignationId = model.DesignationId;
                        doctor.SubscriptionlId = model.SubscriptionlId;

                        doctor.IsActive = false;
                        if (_doctorService.Update(doctor))
                        {
                            await SignInAsync(identityUser, doctor, true);
                            _message.success(this, "Account Updated Successfully!");

                            if (doctor.Subscription != null && doctor.Subscription.Cost > 0)
                                return RedirectToAction("Fees", "Payments", new { doctorId = doctor.Id });
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        if (image != null)
                        {
                            string ImageName = UploadImage(image, identityUser.UserName);
                            model.Image = ImageName;
                        }

                        if (_doctorService.Add(model))
                        {
                            _message.success(this, "Account Updated Successfully!");
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                else
                {
                    _message.custom(this, "Email already used.");
                    return View(model);
                }
            }
            else
            {
                _message.custom(this, passwordChageResult.Errors.ToString());
                return View(model);
            }



            _message.custom(this, "Error! No data saved");
            return View(model);
        }

        private string UploadImage(HttpPostedFileBase image, string mobileNo)
        {
            try
            {
                var extension = "." + image.FileName.Split('.')[image.FileName.Split('.').Length - 1];
                string fileName = "image_" + mobileNo + extension; //Create a new Name for the file due to security reasons.

                var filePathOriginal = Request.MapPath(Request.ApplicationPath) + @"/Content/Upload/images";
                string savedOrgFileName = Path.Combine(filePathOriginal, fileName);

                if (!Directory.Exists(filePathOriginal))
                {
                    Directory.CreateDirectory(filePathOriginal);
                }

                if (System.IO.File.Exists(savedOrgFileName))
                {
                    System.IO.File.Delete(savedOrgFileName);
                }

                image.SaveAs(savedOrgFileName);
                return fileName;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        [HttpGet]
        public ActionResult IsStudentOrIntern(int designationId)
        {
            var designation = _designationService.GetDetails(designationId);
            if (designation.IsSubscriptionFree)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetSubscriptions(int designationId, int doctorId)
        {
            List<Subscription> subscriptions = new List<Subscription>();
            var doctor = _doctorService.GetDetails(doctorId);
            var designation = _designationService.GetDetails(designationId);
            if (designation.IsSubscriptionFree)
            {
                subscriptions = _subscriptionService.GetAll().Where(s => s.Cost == 0).ToList();
            }
            else
            {
                subscriptions = _subscriptionService.GetAll().Where(s => s.Cost != 0).ToList();
            }

            if (subscriptions != null && subscriptions.Count() > 0)
            {
                if (doctor.SubscriptionlId != null && doctor.SubscriptionlId > 0 && doctor.DesignationId != null && doctor.DesignationId == designationId)
                {

                    foreach (var item in subscriptions)
                    {
                        if (doctor.SubscriptionlId != null && doctor.SubscriptionlId > 0)
                        {
                            if (item.Id == doctor.SubscriptionlId)
                            {
                                item.IsChecked = true;
                            }
                        }
                    }
                }
                else
                {
                    subscriptions.FirstOrDefault().IsChecked = true;
                }
            }
            return View("_SubscriptionPartial", subscriptions);
        }


        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        [HttpGet]
        [AppAuthorization]
        public ActionResult AccountInfo()
        {
            var doctor = _doctorService.GetDoctorByAppUserID(User.Identity.GetUserId());
            ViewBag.UpgradeSubscriptionContactNo = ConfigurationManager.AppSettings["UpgradeSubscriptionContactNo"];
            return View(doctor);
        }

        [HttpGet]
        [AppAuthorization]
        public ActionResult EditAccountInfo(int id)
        {
            if (id != User.GETDOCTORID())
            {
                return RedirectToAction("UnAuthorized", "Account");
            }
            else
            {
                var doctor = _doctorService.GetDetails(id);
                return View(doctor);
            }
        }

        [HttpGet]
        public ActionResult EditClinicInfo(int doctorId)
        {
            var doctor = _doctorService.GetDetails(doctorId);
            return View("_EditClinicInfo", doctor);
        }

        [HttpPost]
        public ActionResult EditClinicInfo(Doctor doctor)
        {
            var doctorDb = _doctorService.GetDetails(doctor.Id);
            doctorDb.ClinicName = doctor.ClinicName;
            doctorDb.ClinicAddress = doctor.ClinicAddress;
            if (_doctorService.Update(doctorDb))
            {
                return Json(new { status = true, message = "Data Updated Successfully" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false, message = "No Data Updated" }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult EditPassword(int doctorId)
        {
            VMChangePassword vm = new VMChangePassword();
            vm.DoctorId = doctorId;
            return View("_EditPassword", vm);
        }

        [HttpPost]
        public async Task<ActionResult> EditPassword(VMChangePassword vm)
        {
            var doctorDb = _doctorService.GetDetails(vm.DoctorId);
            var identityUser = await UserManager.FindByIdAsync(doctorDb.AppUserID);

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), vm.CurrentPassword, vm.Password);
            if (result.Succeeded)
            {
                return Json(new { status = true, message = "Password Updated Successfully" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { status = false, message = "Current password is incorrect" }, JsonRequestBehavior.AllowGet);

            }

            return Json(new { status = false, message = "No Data Updated" }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult EditEmail(int doctorId)
        {
            var doctor = _doctorService.GetDetails(doctorId);
            VMChangeEmail vm = new VMChangeEmail();
            vm.DoctorId = doctorId;
            vm.CurrentEmail = doctor.AppUser.Email;
            return View("_EditEmail", vm);
        }

        [HttpPost]
        public async Task<ActionResult> EditEmail(VMChangeEmail vm)
        {
            var doctorDb = _doctorService.GetDetails(vm.DoctorId);
            var identityUser = await UserManager.FindByIdAsync(doctorDb.AppUserID);
            if (vm.CurrentEmail == vm.Email)
            {
                vm.Message = "Current mail and new mail are same.";
                return View("_EditEmail", vm);
            }
            var userWithEMail = await UserManager.FindByEmailAsync(vm.Email);
            if (userWithEMail != null)
            {
                vm.Message = "Email already used.";
                return View("_EditEmail", vm);
            }
            Random _random = new Random();
            var emailOtp = _random.Next(100000, 999999);
            identityUser.EmailOtp = emailOtp.ToString();

            var updated = await UserManager.UpdateAsync(identityUser);
            if (updated.Succeeded)
            {
                if (EmailGateway.SendEmail(vm.Email, emailOtp.ToString()))
                {
                    return View("_EditEmailOtp", vm);
                }
                else
                {
                    vm.Message = "Failed to send otp.";
                    return View("_EditEmail", vm);
                }

                //return Json(new { status = true, message = "OTP Send " }, JsonRequestBehavior.AllowGet);
            }
            vm.Message = "Error! Something bad happend.";
            return View("_EditEmail", vm);
            //return Json(new { status = false, message = "No Data Updated" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> SubmitEmailOtp(VMChangeEmail vm)
        {
            var doctorDb = _doctorService.GetDetails(vm.DoctorId);
            var identityUser = await UserManager.FindByIdAsync(doctorDb.AppUserID);


            if (identityUser.EmailOtp == vm.EmailOtp)
            {
                identityUser.Email = vm.Email;
                UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager)
                {
                    RequireUniqueEmail = true
                };
                var updated = await UserManager.UpdateAsync(identityUser);
                if (updated.Succeeded)
                {
                    return Json(new { status = true, message = "Data Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = true, message = "Email already used." }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { status = false, message = "Invalid try" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { status = false, message = "No Data Updated" }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult EditMobileNo(int doctorId)
        {
            var doctor = _doctorService.GetDetails(doctorId);
            VMChangeMobileNo vm = new VMChangeMobileNo();
            vm.DoctorId = doctorId;
            vm.CurrentMobileNo = doctor.AppUser.UserName;
            return View("_EditMobileNo", vm);
        }

        [HttpPost]
        public async Task<ActionResult> EditMobileNo(VMChangeMobileNo vm)
        {
            var doctorDb = _doctorService.GetDetails(vm.DoctorId);
            var identityUser = await UserManager.FindByIdAsync(doctorDb.AppUserID);

            var key = KeyGeneration.GenerateRandomKey(10);
            string randkey = Convert.ToBase64String(key);
            var totp = new Totp(key, step: _otpConfig.Time, totpSize: _otpConfig.Length, timeCorrection: null);
            var otp = totp.ComputeTotp(DateTime.UtcNow);

            identityUser.Otp = otp;
            identityUser.OtpKey = randkey;
            var updated = await UserManager.UpdateAsync(identityUser);
            if (updated.Succeeded)
            {
                string message = "Use " + otp + " as your OTP to change mobile no. Thank you.";
                var res = SMSGateway.SendSMS(message, vm.MobileNo);
                if (res.status_code == 200)
                {
                    return View("_EditMobileNoOtp", vm);
                }
                else
                {
                    return View("_EditMobileNo", vm);
                }


            }
            return View("_EditMobileNo", vm);

        }

        [HttpPost]
        public async Task<ActionResult> SubmitMobileNoOtp(VMChangeMobileNo vm)
        {
            var doctorDb = _doctorService.GetDetails(vm.DoctorId);
            var identityUser = await UserManager.FindByIdAsync(doctorDb.AppUserID);

            byte[] Bytekey = Convert.FromBase64String(identityUser.OtpKey);
            var totp = new Totp(Bytekey, step: _otpConfig.Time, totpSize: _otpConfig.Length, timeCorrection: null);
            long timeStepMatched;

            bool verify = totp.VerifyTotp(DateTime.UtcNow, vm.MobileNoOtp, out timeStepMatched, VerificationWindow.RfcSpecifiedNetworkDelay);

            if (verify)
            {
                identityUser.UserName = vm.MobileNo;
                identityUser.Otp = "";
                identityUser.OtpKey = "";
                var updated = await UserManager.UpdateAsync(identityUser);
                if (updated.Succeeded)
                {
                    return Json(new { status = true, message = "Data Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { status = false, message = "Invalid try" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { status = false, message = "No Data Updated" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditDesignation(int doctorId)
        {
            var doctor = _doctorService.GetDetails(doctorId);
            VMChangeDesignation vm = new VMChangeDesignation();
            vm.DoctorId = doctorId;
            vm.DesignationId = doctor.DesignationId ?? 0;
            vm.BMDCRegistrationNumber = doctor.BMDCRegistrationNumber;
            ViewBag.DesignationId = new SelectList(_designationService.GetAll(), "Id", "Name", vm.DesignationId);
            return View("_EditDesignation", vm);
        }
        [HttpPost]
        public ActionResult EditDesignation(VMChangeDesignation vm)
        {
            var doctorDb = _doctorService.GetDetails(vm.DoctorId);
            doctorDb.DesignationId = vm.DesignationId;
            

            string msg = "";
            if (doctorDb.BMDCRegistrationNumber != vm.BMDCRegistrationNumber)
            {
                msg = @"Thank you for your request for BMDC Registration Number update. You will be notified via SMS in 24-48 hours on whether your request is successful";
                doctorDb.IsBMDCVerified = false;
            }
            else
            {
                msg = @"Data Updated Successfully";
            }
            doctorDb.BMDCRegistrationNumber = vm.BMDCRegistrationNumber;
            if (_doctorService.Update(doctorDb))
            {
                return Json(new { status = true, message = msg }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false, message = "No Data Updated" }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult UnAuthorized()
        {
            return View();
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        //private IAuthenticationManager AuthenticationManager
        //{
        //    get
        //    {
        //        return HttpContext.GetOwinContext().Authentication;
        //    }
        //}

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}