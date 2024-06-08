using Pickfc.BLL.Interfaces;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Pickfc.Model.DTOs;

namespace Pickfc.BLL.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration config;
        public MailService(IConfiguration config) {
            this.config = config;
        }

        public void CodeRequest(string email, string code, bool activationCode) {

            var msg = new Message { 
                To = email,
            };
            if (activationCode)
            {
                msg.Subject = "Account Activation";
                msg.Content = $"<p>Use the code below to activate your acount.</p><br/><b>{code}</b>";
            }
            else {
                msg.Subject = "Password Reset";
                msg.Content = $"<p>Use the code below to reset your password.</p><br/><b>{code}</b>";
            }
            Send(Sending(msg));
        }

        public void RoundDeadline(string email) {
            var msg = new Message {
                To = email,
                Subject = "Round Deadline",
                Content = "A game round is fast approching it's deadline. " + $"<a href=\"{config.GetSection("ClientUrl:Web").Value}\">Pick your team before time runs out!</a></span>",
            };
            Send(Sending(msg));
        }

        public void NewContent(string email, string content) {
            var msg = new Message
            {
                To = email,
                Subject = "New " + content + " availible",
                Content = "A new " + content + " is now availible. " + $"<a href=\"{config.GetSection("ClientUrl:Web").Value}\">Click to play</a>"
            };
            Send(Sending(msg));
        }

        private MimeMessage Sending(Message message) { 

            var msg = new MimeMessage();
            msg.From.Add(MailboxAddress.Parse(config.GetSection("Mail:From").Value));
            msg.To.Add(MailboxAddress.Parse(message.To));
            msg.Subject = message.Subject;
            msg.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = string.Format(message.Content)};

            return msg;
        }

        private void Send(MimeMessage msg) {
            using (var stmp = new SmtpClient())
            {
                stmp.Connect(config.GetSection("Mail:Host").Value, int.Parse(config.GetSection("Mail:Port").Value), MailKit.Security.SecureSocketOptions.StartTls);
                stmp.Authenticate(config.GetSection("Mail:Username").Value, config.GetSection("Mail:Password").Value);
                
                stmp.Send(msg);
                stmp.Disconnect(true);
            }    
        }
    }
}
