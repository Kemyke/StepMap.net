using StepMap.BusinessLogic;
using dal = StepMap.DAL;
using StepMap.ServiceContracts;
using dto = StepMap.ServiceContracts.DTO;
using StepMap.ServiceImpl.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace StepMap.ServiceImpl
{
    [ServiceBehavior(Namespace = "http://kemy.com", InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class StepMapService : IStepMapService
    {
        private readonly IProjectManager projectManager;
        private readonly IUserManager userManager;
        private readonly IOperationContextProvider operationContextProvider;

        public StepMapService(IProjectManager projectManager, IUserManager userManager, IOperationContextProvider operationContextProvider)
        {
            this.projectManager = projectManager;
            this.userManager = userManager;
            this.operationContextProvider = operationContextProvider;
        }

        public IList<dto.Project> GetProjects()
        {
            dal.User currentUser = operationContextProvider.CurrentUser;
            IEnumerable<dal.Project> projects = projectManager.GetProjects(currentUser);

            return projects.Select(p => ProjectConverter.ConvertProject(p)).ToList();
        }
    }
}
