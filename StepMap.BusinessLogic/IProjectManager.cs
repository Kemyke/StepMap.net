using StepMap.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.BusinessLogic
{
    public interface IProjectManager
    {
        IEnumerable<Project> GetProjects(User user);
    }
}
