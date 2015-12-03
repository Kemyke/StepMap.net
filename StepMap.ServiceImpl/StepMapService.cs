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
using StepMap.Common;
using StepMap.Common.Exceptions;

namespace StepMap.ServiceImpl
{
    [ServiceBehavior(Namespace = "http://stepmap.xyz", InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, AddressFilterMode = AddressFilterMode.Any)]
    public class StepMapService : IStepMapService
    {
        private readonly ILogger logger;
        private readonly IProjectManager projectManager;
        private readonly IUserManager userManager;
        private readonly IOperationContextProvider operationContextProvider;
        private readonly IManagementManager managementManager;

        public StepMapService(ILogger logger, IProjectManager projectManager, IManagementManager managementManager, IUserManager userManager, IOperationContextProvider operationContextProvider)
        {
            this.logger = logger;
            this.projectManager = projectManager;
            this.userManager = userManager;
            this.operationContextProvider = operationContextProvider;
            this.managementManager = managementManager;

            logger.Info("{0} created!", this.GetType().Name);
        }

        public Response<IList<dto.Project>> GetProjects()
        {
            return WrapResponse<IList<dto.Project>>(() =>
            {
                logger.Debug("GetProjects called");

                dal.User currentUser = operationContextProvider.CurrentUser;
                IEnumerable<dal.Project> projects = projectManager.GetProjects(currentUser);

                return projects.Select(p => p == null ? null : ProjectConverter.ConvertProject(p)).ToList();
            });
        }


        public Response AddProject(dto.Project project)
        {
            return WrapResponse(() =>
            {
                dal.Project dalProj = ProjectConverter.ConvertProject(project);
                dalProj.UserId = operationContextProvider.CurrentUser.Id;
                projectManager.AddProject(dalProj);

            });
        }

        public Response UpdateProject(dto.Project project)
        {
            return WrapResponse(() =>
            {
                dal.Project dalProj = ProjectConverter.ConvertProject(project);
                projectManager.UpdateProject(dalProj);
            });
        }

        public Response DeleteProject(int projectId)
        {
            return WrapResponse(() =>
            {
                projectManager.DeleteProject(projectId);
            });
        }


        public Response<dto.User> Login()
        {
            return WrapResponse(() =>
                {
                    dto.User ret;
                    ret = UserConverter.ConvertUser(operationContextProvider.CurrentUser);
                    return ret;
                });
        }

        public Response Register(string userName, string email, string password)
        {
            return WrapResponse(() =>
                {
                    userManager.Register(userName, email, password);
                });
        }

        public Response CheckDeadlines()
        {
            return WrapResponse(() =>
            {
                managementManager.CheckAllProjectsProgress();
            });
        }

        public Response<dto.User> ConfirmEmail(string guid)
        {
            return WrapResponse(() =>
            {
                dal.User ret = userManager.ConfirmEmail(guid);
                return UserConverter.ConvertUser(ret);
            });
        }

        private Response<T> WrapResponse<T>(Func<T> method)
        {
            ResultCode rc = ResultCode.OK;
            T ret = default(T);
            try
            {
                ret = method();
            }
            catch(ConfirmationGuidNotValidException ex)
            {
                logger.Error(ex.ToString());
                rc = ResultCode.CONFIRMATION_GUID_NOT_VALID;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                rc = ResultCode.UNKOWN_ERROR;
            }
            return new Response<T>(rc, ret);
        }

        private Response WrapResponse(Action method)
        {
            var ret = WrapResponse<object>(() => { method(); return null; });
            return new Response(ret.ResultCode);
        }
    }
}
