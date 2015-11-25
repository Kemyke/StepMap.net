using StepMap.Common;
using StepMap.ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.ServiceContracts
{
    [ServiceContract]
    public interface IStepMapService
    {
        /// <summary>
        /// Get projects
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/projects", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IList<Project> GetProjects();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/projects", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void AddProject(Project project);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "/projects", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void UpdateProject(Project project);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "/projects", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void DeleteProject(int projectId);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/accounts", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        User Login();

        [OperationContract]
        [DoNotAuthorizeAttribute]
        [WebInvoke(Method = "POST", UriTemplate = "/accounts", BodyStyle=WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void Register(string userName, string email, string password);
    }
}
