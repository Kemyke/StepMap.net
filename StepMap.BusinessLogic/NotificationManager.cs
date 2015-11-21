using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StepMap.DAL;
using System.Net;
using System.Threading;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using StepMap.Logger.Logging;
using System.Security.Cryptography.X509Certificates;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2.Requests;
using System.IO;
using Google.Apis.Util.Store;
using Google.Apis.Gmail.v1.Data;
using System.Net.Mail;

namespace StepMap.BusinessLogic
{
    public class NotificationManager : INotificationManager
    {
        private readonly ILogger logger;
        private readonly GmailService gmailService;

        public NotificationManager(ILogger logger)
        {
            this.logger = logger;
            UserCredential credential = CreateCredential();
            gmailService = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

        }
        static string[] Scopes = { GmailService.Scope.GmailSend };
        static string ApplicationName = "stepmap";

        private UserCredential CreateCredential()
        {
            UserCredential credential;
            using (var stream = new FileStream("client_secret_114882284440-unrvt3i9sbfakciqr3rv2a4cs476321d.apps.googleusercontent.com.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "stepmap.daemon@gmail.com",
                    CancellationToken.None).Result;
            }
            return credential;
        }

        private static string Base64UrlEncode(string input)
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            // Special "url-safe" base64 encode.
            return Convert.ToBase64String(inputBytes)
              .Replace('+', '-')
              .Replace('/', '_')
              .Replace("=", "");
        }
        public void SendEmail(User user, string subject, string text)
        {
            var msg = new AE.Net.Mail.MailMessage
            {
                Subject = subject,
                Body = text,
                From = new MailAddress("stepmap.daemon@gmail.com")
            };
            msg.To.Add(new MailAddress(user.Email));
            msg.ReplyTo.Add(msg.From); // Bounces without this!!
            var msgStr = new StringWriter();
            msg.Save(msgStr);

            var m = new Message { Raw = Base64UrlEncode(msgStr.ToString()) };
            gmailService.Users.Messages.Send(m, "me").Execute();
        }
    }
}
