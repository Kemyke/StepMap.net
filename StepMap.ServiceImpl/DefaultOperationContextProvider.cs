using StepMap.BusinessLogic;
using System;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace StepMap.ServiceImpl
{
    public class DefaultOperationContextProvider : IOperationContextProvider
    {
        private readonly IUserManager userManager;

        public DefaultOperationContextProvider(IUserManager userManager)
        {
            if(userManager == null)
            {
                throw new ArgumentNullException("userManager");
            }
            this.userManager = userManager;
        }

        private string GetLoginName()
        {
            var authHeader = WebOperationContext.Current.IncomingRequest.Headers["Authorization"];
            var svcCredentials = System.Text.ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(authHeader.Substring(6))).Split(':');
            var userName = svcCredentials[0];
            return userName;
        }

        public DAL.User CurrentUser
        {
            get
            {
                var userName = GetLoginName();
                return userManager.GetUser(userName);
            }
        }
    }
}