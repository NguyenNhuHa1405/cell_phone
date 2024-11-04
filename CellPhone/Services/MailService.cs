using CellPhone.Migrations;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CellPhone.Services
{
    public class MailService
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var mail = ConfigurationManager.AppSettings.Get("Mail");
            var displayName = ConfigurationManager.AppSettings.Get("DisplayName");
            var password = ConfigurationManager.AppSettings.Get("Password");
            var host = ConfigurationManager.AppSettings.Get("Host");
            int.TryParse(ConfigurationManager.AppSettings.Get("Port"), out var port);
            var mimeMessage = new MimeMessage();
            var body = new BodyBuilder();
            body.HtmlBody = htmlMessage;
            mimeMessage.Sender = new MailboxAddress(displayName, mail);
            mimeMessage.From.Add(new MailboxAddress(displayName, mail));
            mimeMessage.To.Add(MailboxAddress.Parse(email));
            mimeMessage.Subject = subject;
            mimeMessage.Body = body.ToMessageBody();
            var sendMail = new SmtpClient();
            await sendMail.ConnectAsync(host, port, SecureSocketOptions.StartTls);
            await sendMail.AuthenticateAsync(mail, password);
            await sendMail.SendAsync(mimeMessage);
            await sendMail.DisconnectAsync(true);
        }
    }
}