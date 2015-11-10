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
    }
}
