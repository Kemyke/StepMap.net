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
using StepMap.Common;
using StepMap.Common.Configuration;

namespace StepMap.BusinessLogic
{
    public class NotificationManager : INotificationManager
    {
        private readonly ILogger logger;
        private readonly GmailService gmailService;
        private readonly IStepMapConfig config;

        public NotificationManager(ILogger logger, IStepMapConfig config)
        {
            this.logger = logger;
            this.config = config;

            UserCredential credential = CreateCredential();
            gmailService = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

        }
        
        private static string[] Scopes = { GmailService.Scope.GmailSend };
        private static string ApplicationName = "stepmap";

        private UserCredential CreateCredential()
        {
            ClientSecrets clientSecrets = new ClientSecrets()
            {
                ClientId =  config.GmailClientId, 
                ClientSecret = config.GmailApiClientSecret
            };

            UserCredential credential;
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                clientSecrets,
                Scopes,
                config.NotificationAccount,
                CancellationToken.None).Result;
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
                From = new MailAddress(config.NotificationAccount)
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
