using StepMap.BusinessLogic;
using StepMap.Common;
using StepMap.Common.DIContainer;
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
        private ILogger logger;
        private IUserManager userManager;

        private const string AuthHeaderKey = "Authorization";
        private readonly Type serviceContractType = typeof(IStepMapService);

        public MyServiceAuthorizationManager()
        {
            DIContainerFactory factory = new DIContainerFactory();
            IDIContainer diContainer = factory.CreateAndLoadDIContainer();

            logger = diContainer.GetInstance<ILogger>();
            userManager = diContainer.GetInstance<IUserManager>();
            userManager = new UserManager();
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
                        throw new WebFaultException(HttpStatusCode.Unauthorized);
                    }
                    else
                    {
                        return success;
                    }
                }
                else
                {
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
