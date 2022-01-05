using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace EPrescribing.Web.Helpers
{
    public static class EmailGateway
    {
        public static bool SendEmail(string receiver, string otp)
        {
            try
            {
                var senderEmailAddress = ConfigurationManager.AppSettings["SenderEmailAddress"];
                var senderDisplayName = ConfigurationManager.AppSettings["SenderDisplayName"];
                var senderEmailPassword = ConfigurationManager.AppSettings["SenderEmailPassword"];

                var senderEmail = new MailAddress(senderEmailAddress, senderDisplayName);
                var receiverEmail = new MailAddress(receiver, "Receiver");

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, senderEmailPassword)
                };
                using (var mess = new MailMessage(senderEmail, receiverEmail)
                {
                    Subject = "OTP",
                    Body = "Email confirmatoin otp-" + otp
                })
                {
                    smtp.Send(mess);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}