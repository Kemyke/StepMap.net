﻿using StepMap.BusinessLogic;
using StepMap.Common;
using StepMap.Common.DIContainer;
using StepMap.Common.RegexHelpers;
using StepMap.Logger.Logging;
using StepMap.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.ServiceImpl
{
    public class MyServiceAuthorizationManager : ServiceAuthorizationManager
    {
        private class MockNotif : INotificationManager
        {
            public void SendEmail(DAL.User user, string subject, string text)
            {
                throw new NotImplementedException();
            }
        }

        private class Config : IStepMapConfig
        {
            public string NotificationAccount
            {
                get { throw new NotImplementedException(); }
            }

            public string GmailClientId
            {
                get { throw new NotImplementedException(); }
            }

            public string GmailApiClientSecret
            {
                get { throw new NotImplementedException(); }
            }

            public string ClientBaseAddress
            {
                get { throw new NotImplementedException(); }
            }

            public string AppPath
            {
                get { throw new NotImplementedException(); }
            }
        }


        private ILogger logger;
        private IUserManager userManager;

        private const string AuthHeaderKey = "Authorization";
        private readonly Type serviceContractType = typeof(IStepMapService);

        public MyServiceAuthorizationManager()
        {
            DIContainerFactory factory = new DIContainerFactory();
            IDIContainer diContainer = factory.CreateAndLoadDIContainer();

            var regexHelper = diContainer.GetInstance<IRegexHelper>();
            logger = diContainer.GetInstance<ILogger>();
            //userManager = diContainer.GetInstance<IUserManager>();
            userManager = new UserManager(logger, new Config(), regexHelper, new MockNotif());
        }

        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            try
            {
                string authHeader = null;

                // ignore authentication for GetVersionInfo call
                MessageProperties msgProps = OperationContext.Current.IncomingMessageProperties;

                //TODO: do not hardcode interface type, use somthing like: OperationContext.Current.EndpointDispatcher.DispatchRuntime.Type.GetInterfaces()
                //Only works with distinct metthod names (no overload allowed)
                if (msgProps != null && msgProps.ContainsKey("HttpOperationName"))
                {
                    string methodName = msgProps["HttpOperationName"].ToString();
                    var mi = serviceContractType.GetMethod(methodName);
                    if (mi != null && mi.GetCustomAttributes(typeof(DoNotAuthorizeAttribute), false).Any())
                    {
                        return base.CheckAccessCore(operationContext);
                    }
                }

                authHeader = WebOperationContext.Current.IncomingRequest.Headers.Get(AuthHeaderKey);

                if ((authHeader != null) && (authHeader != string.Empty))
                {
                    var svcCredentials = System.Text.ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(authHeader.Substring(6))).Split(':');
                    var userName = svcCredentials[0];
                    var password = svcCredentials[1];

                    bool success = userManager.IsPasswordValid(userName, password);
                    if (!success)
                    {
                        logger.Error("Invalid password: {0}, {1}", userManager, password);
                        throw new WebFaultException(HttpStatusCode.Unauthorized);
                    }
                    else
                    {
                        logger.Debug("Succesfully authenticated: {0}", authHeader);
                        return success;
                    }
                }
                else
                {
                    logger.Error("Auth header is empty!");
                    throw new WebFaultException(HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                throw new WebFaultException(HttpStatusCode.Unauthorized);
            }
        }
    }
}
