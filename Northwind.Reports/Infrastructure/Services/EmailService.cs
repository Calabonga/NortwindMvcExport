using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Calabonga.Portal.Config;
using Northwind.Web;

namespace Alimana.Web.Infrastructure {

    public class EmailService : IEmailService {
        private readonly IConfigService<CurrentAppSettings> _configService;
        private readonly string _robotMail;
        private string _adminMail;

        public EmailService(IConfigService<CurrentAppSettings> configService) {
            _configService = configService;
            _robotMail = configService.Config.RobotEmail;
            _adminMail = configService.Config.AdminEmail;
        }

        public bool SendMail(string mailFrom, string mailto, string mailSubject, string mailBody, IEnumerable<HttpPostedFileBase> files) {
            return SendMailFromServer(mailFrom, mailto, mailSubject, mailBody, files, true);
        }

        public Task SendMailAsync(string mailFrom, string mailto, string mailSubject, string mailBody, IEnumerable<HttpPostedFileBase> files) {
            return SendMailFromServerAsync(mailFrom, mailto, mailSubject, mailBody, files, true);
        }

        public bool SendMail(string mailFrom, string mailto, string mailSubject, string mailBody) {
            return SendMailFromServer(mailFrom, mailto, mailSubject, mailBody, null, true);
        }

        public Task SendMailAsync(string mailFrom, string mailto, string mailSubject, string mailBody) {
            return SendMailFromServerAsync(mailFrom, mailto, mailSubject, mailBody, null, true);
        }

        public bool SendMail(string mailto, string mailSubject, string mailBody) {
            return SendMailFromServer(_robotMail, mailto, mailSubject, mailBody, null, true);
        }

        public Task SendMailAsync(string mailto, string mailSubject, string mailBody) {
            return SendMailFromServerAsync(_robotMail, mailto, mailSubject, mailBody, null, true);
        }

        public bool NotifyAdmin(string mailSubject, string mailBody) {
            return SendMail(_adminMail, mailSubject, mailBody);
        }

        public Task NotifyAdminAsync(string mailSubject, string mailBody) {
            return SendMailFromServerAsync(_robotMail, _adminMail, mailSubject, mailBody, null, true);
        }

        public void SendMail(IEmailMessage message) {
            SendMail(_robotMail, message.MailTo, message.Subject, message.Body);
        }

        public Task SendMailAsync(IEmailMessage message) {
            return SendMailFromServerAsync(_robotMail, message.MailTo, message.Subject, message.Body, null, true);
        }

        private bool SendMailFromServer(string mailFrom, string mailto, string mailSubject, string mailBody, IEnumerable<HttpPostedFileBase> files, bool isHtml) {
            bool mailSended;
            try {
                using (var message = new MailMessage(mailFrom, mailto, mailSubject, mailBody)) {
                    if (files != null) {
                        foreach (var file in files) {
                            var data = new Attachment(file.InputStream, file.ContentType);
                            var disposition = data.ContentDisposition;
                            disposition.CreationDate = File.GetCreationTime(file.FileName);
                            disposition.ModificationDate = File.GetLastWriteTime(file.FileName);
                            disposition.ReadDate = File.GetLastAccessTime(file.FileName);
                            message.Attachments.Add(data);
                        }
                    }
                    message.BodyEncoding = Encoding.UTF8;
                    message.SubjectEncoding = Encoding.UTF8;
                    message.IsBodyHtml = isHtml;

                    using (var client = new SmtpClient("localhost")) {
                        client.Credentials = CredentialCache.DefaultNetworkCredentials;
                        client.Send(message);
//#if !DEBUG
//#endif
                    }
                }
                mailSended = true;
            }
            catch (SmtpException exception) {
                mailSended = false;
            }
            return mailSended;
        }

        private Task SendMailFromServerAsync(string mailFrom, string mailto, string mailSubject, string mailBody, IEnumerable<HttpPostedFileBase> files, bool isHtml) {
            try {
                var message = new MailMessage(mailFrom, mailto, mailSubject, mailBody);
                if (files != null) {
                    foreach (var file in files) {
                        var data = new Attachment(file.InputStream, file.ContentType);
                        var disposition = data.ContentDisposition;
                        disposition.CreationDate = File.GetCreationTime(file.FileName);
                        disposition.ModificationDate = File.GetLastWriteTime(file.FileName);
                        disposition.ReadDate = File.GetLastAccessTime(file.FileName);
                        message.Attachments.Add(data);
                    }
                }
                message.BodyEncoding = Encoding.UTF8;
                message.SubjectEncoding = Encoding.UTF8;
                message.IsBodyHtml = isHtml;

                var client = new SmtpClient("localhost");
                client.SendCompleted += (s, e) => {
                    message.Dispose();
                    client.Dispose();
                };
                client.Credentials = CredentialCache.DefaultNetworkCredentials;

#if !DEBUG
                return Task.Factory.StartNew(() => client.Send(message));
#else
                return Task.FromResult(0);
#endif


            }
            catch
            {
                // ignored
            }
            return Task.FromResult(0);
        }
    }
}