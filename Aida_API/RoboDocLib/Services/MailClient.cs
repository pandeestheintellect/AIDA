using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RoboDocLib.Services
{
    public class MailClient
    {
        public string SendMailFromGmail1(string AliasName, string To, string Subject, string Body,List<string>attachments)
        {
            return SendMailGmail1(AliasName, To, Subject, Body, attachments);
        }
        public string SendMailFromGmail1 (string AliasName, string To, string Subject, string Body)
        {
            return SendMailAchi(AliasName, To, Subject, Body, null);
        }

        private string SendMailAchi(string AliasName, string To, string Subject, string Body, List<string> attachments)
        {
            using (var smtpClient = new SmtpClient("mail.achibiz.com", 26))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential()
                {
                    UserName = "otp@achibiz.com",
                    Password = "h]-C26DY)rbp",
                };
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;

                MailMessage message = new MailMessage();
                message.From = new MailAddress("otp@achibiz.com", AliasName);

                message.To.Add(new MailAddress(To));
                message.Subject = Subject;
                message.Body = Body;
                message.IsBodyHtml = true;
                if (attachments != null)
                {
                    foreach (string files in attachments)
                    {
                        message.Attachments.Add(new Attachment(files));
                    }
                }
                //smtpClient.Send ("jminfosys.sg@gmail.com", To, Subject, Body);

                try
                {
                    smtpClient.Send(message);
                    return "OKAY";
                }
                catch (Exception e)
                {
                    return "Mail Send Failed " + e.Message;
                }
            }
        }
        private string SendMailGmail1(string AliasName, string To, string Subject, string Body, List<string> attachments)
        {
            using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential()
                {
                    UserName = "jminfosys.sg@gmail.com",
                    Password = "JMInfo#2020",
                };
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;

                MailMessage message = new MailMessage();
                message.From = new MailAddress("jminfosys.sg@gmail.com", AliasName);

                message.To.Add(new MailAddress(To));
                message.Subject = Subject;
                message.Body = Body;
                message.IsBodyHtml = true;
                if (attachments!=null)
                {
                    foreach(string files in attachments)
                    {
                        message.Attachments.Add(new Attachment(files));
                    }
                }
                //smtpClient.Send ("jminfosys.sg@gmail.com", To, Subject, Body);

                try
                {
                    smtpClient.Send(message);
                    return "OKAY";
                }
                catch (Exception e)
                {
                    return "Mail Send Failed " + e.Message;
                }
            }
        }

        public string SendMail(string AliasName, string To, string Subject, string Body)
        {
            return SendMailAchi(AliasName, To, Subject, Body, null);
        }
        public string SendMail(string AliasName, string To, string Subject, string Body, List<string> attachments)
        {
            return SendMailAchi(AliasName, To, Subject, Body, attachments);
        }
        public string SendMail1(string AliasName, string To, string Subject, string Body, List<string> attachments)
        {

            MailMessage message = new MailMessage();
            message.From = new MailAddress("samy@jcs-asia.com", AliasName);

            message.To.Add(new MailAddress(To));
            message.Subject = Subject;
            message.Body = Body;
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient("relay-hosting.secureserver.net", 25);
            client.UseDefaultCredentials = true;
            client.EnableSsl = false;
            // drop the @domain stuff from your user name: (The API already knows the domain
            // from the construction of the SmtpClient instance
            client.Credentials = new NetworkCredential("samy@jcs-asia.com", "Jcs-asia@2018");

            if (attachments != null)
            {
                foreach (string files in attachments)
                {
                    message.Attachments.Add(new Attachment(files));
                }
            }

            try
            {
                client.Send(message);
                return Subject;
            }
            catch (Exception e)
            {
                return "Mail Send Failed " + e.Message;
            }

        }
    }
}
