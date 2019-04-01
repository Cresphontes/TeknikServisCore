using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TeknikServisCore.Models.Enums;
using TeknikServisCore.Models.ViewModels;

namespace TeknikServisCore.BLL.Services
{
    public class EmailService : IMessageService
    {
        public MessageStates MessageStates { get; private set; }
        public string SenderMail { get; set; }
        public string FilePath { get; set; }
        public string[] CC { get; set; }
        public string[] BCC { get; set; }
        public string Smtp { get; set; }
        public string Password { get; set; }
        public int SmtpPort { get; set; }


        public EmailService()
        {
            this.SenderMail = "ab.service3922@gmail.com";
            this.Password = "abservice3922.";
            this.Smtp = "smtp.gmail.com";
            this.SmtpPort = 587;
        }

        public EmailService(string _senderMail,string _password, string _smtp,int _smtpPort)
        {
            this.SenderMail = _senderMail;
            this.Password = _password;
            this.Smtp = _smtp;
            this.SmtpPort = _smtpPort;
        }

        public async Task SendAsync(MessageViewModel message, params string[] contacts)
        {
            try
            {
                var mail = new MailMessage()
                {
                    From = new MailAddress(SenderMail)
                };


                if (!string.IsNullOrEmpty(FilePath))
                {
                    mail.Attachments.Add(new Attachment(FilePath));
                }

                if (CC != null && CC.Length > 0)
                {
                    foreach (var cc in CC)
                    {
                        mail.To.Add(cc);
                    }
                }

                if (BCC != null && BCC.Length > 0)
                {
                    foreach (var bcc in BCC)
                    {
                        mail.To.Add(bcc);
                    }
                }

                foreach (var c in contacts)
                {
                    mail.To.Add(c);
                }

                mail.Subject = message.Subject;
                mail.Body = message.Body;

                mail.IsBodyHtml = true;
                mail.BodyEncoding = Encoding.UTF8;
                mail.HeadersEncoding = Encoding.UTF8;
                mail.SubjectEncoding = Encoding.UTF8;

                var smtpClient = new SmtpClient(this.Smtp, this.SmtpPort)
                {
                    Credentials = new NetworkCredential(this.SenderMail, this.Password),
                    EnableSsl = true

                };

                await smtpClient.SendMailAsync(mail);
                MessageStates = MessageStates.Delivered;

            }
            catch (Exception e)
            {
                MessageStates = MessageStates.NotDelivered;
            }
        }
    }
}
