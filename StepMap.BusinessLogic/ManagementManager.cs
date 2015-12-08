using StepMap.DAL;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMap.BusinessLogic
{
    public class ManagementManager : IManagementManager
    {
        private readonly IProjectManager projectManager;

        public ManagementManager(IProjectManager projectManager)
        {
            this.projectManager = projectManager;
        }

        public void CheckAllProjectsProgress()
        {
            List<Project> projects;
            using(var ctx = new StepMapDbContext())
            {
                projects = ctx.Projects.Include(p=>p.User).Include(p=>p.FinishedSteps.Select(x=>x.SentReminders)).ToList();
            }

            //Parallel.ForEach(ctx.Projects, (project) =>
            //{
            //    projectManager.CheckProjectProgress(project);
            //});

            foreach(var proj in projects)
            {
                projectManager.CheckProjectProgress(proj);
            }
        }
    }
}
