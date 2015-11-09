using StepMap.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.BusinessLogic
{
    public class ProjectManager : IProjectManager
    {
        public IEnumerable<Project> GetProjects(User user)
        {
            using (var ctx = new StepMapDbContext())
            {
                return ctx.Projects.Where(p => p.UserId == user.Id);
            }
        }
    }
}
