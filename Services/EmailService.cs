using CSP_Redemption_WebApi.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Services
{
    public interface IEmailService
    {
        ResponseModel SendEmail(EmailModel email);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        private readonly string _host;
        private readonly string _from;
        private readonly string _port;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;

            _host = _configuration["SMTP:Host"];
            _from = _configuration["SMTP:From"];
            _port = _configuration["SMTP:Port"];
        }

        public ResponseModel SendEmail(EmailModel email)
        {
            var response = new ResponseModel();
            try
            {
                var emailTo = string.Empty;
                if (email.To.Count <= 0)
                {
                    response.Message = "Feild 'to' is required";
                    return response;
                }
                else
                    email.To = email.To.Distinct().ToList();

                foreach (var to in email.To)
                    emailTo = $"{to.Trim()},";

                if (emailTo.Length > 0)
                    emailTo = emailTo.Remove(emailTo.Length - 1);

                Dictionary<string, string> replacements = new Dictionary<string, string>();
                if (email.Parameters != null)
                {
                    foreach (var parameter in email.Parameters)
                    {
                        replacements.Add($"[#{parameter.Key}#]", parameter.Value);
                    }
                    foreach (var replacement in replacements)
                    {
                        email.Template = email.Template.Replace(replacement.Key, replacement.Value);
                    }
                }

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Port = Convert.ToInt32(_port);
                    smtp.Host = _host;
                    smtp.EnableSsl = false;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                    MailMessage mailMessage = new MailMessage()
                    {
                        BodyEncoding = System.Text.Encoding.UTF8,
                        From = new MailAddress(_from),
                        Sender = new MailAddress(_from),
                        Subject = email.Subject,
                        Body = email.Template,
                        IsBodyHtml = true
                    };

                    if (email.AttachmentPaths != null)
                    {
                        foreach (var attachmentPath in email.AttachmentPaths)
                        {
                            mailMessage.Attachments.Add(new Attachment(attachmentPath));
                        }

                    }

                    mailMessage.To.Add(emailTo);

                    smtp.Send(mailMessage);
                    mailMessage.Dispose();

                    response.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
