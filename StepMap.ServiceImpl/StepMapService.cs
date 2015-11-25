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
using StepMap.Logger.Logging;

namespace StepMap.ServiceImpl
{
    [ServiceBehavior(Namespace = "http://kemy.com", InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, AddressFilterMode = AddressFilterMode.Any)]
    public class StepMapService : IStepMapService
    {
        private readonly ILogger logger;
        private readonly IProjectManager projectManager;
        private readonly IUserManager userManager;
        private readonly IOperationContextProvider operationContextProvider;

        public StepMapService(ILogger logger, IProjectManager projectManager, IUserManager userManager, IOperationContextProvider operationContextProvider)
        {
            this.logger = logger;
            this.projectManager = projectManager;
            this.userManager = userManager;
            this.operationContextProvider = operationContextProvider;

            logger.Info("{0} created!", this.GetType().Name);
        }

        public IList<dto.Project> GetProjects()
        {
            logger.Debug("GetProjects called");

            dal.User currentUser = operationContextProvider.CurrentUser;
            IEnumerable<dal.Project> projects = projectManager.GetProjects(currentUser);

            return projects.Select(p => p == null ? null : ProjectConverter.ConvertProject(p)).ToList();
        }


        public void AddProject(dto.Project project)
        {
            dal.Project dalProj = ProjectConverter.ConvertProject(project);
            dalProj.UserId = operationContextProvider.CurrentUser.Id;
            projectManager.AddProject(dalProj);
        }

        public void UpdateProject(dto.Project project)
        {
            dal.Project dalProj = ProjectConverter.ConvertProject(project);
            projectManager.UpdateProject(dalProj);
        }

        public void DeleteProject(int projectId)
        {
            projectManager.DeleteProject(projectId);
        }


        public dto.User Login()
        {
            dto.User ret;
            ret = UserConverter.ConvertUser(operationContextProvider.CurrentUser);
            return ret;
        }

        public void Register(string userName, string email, string password)
        {
            userManager.Register(userName, email, password);
        }
    }
}
