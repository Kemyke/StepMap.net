using StepMap.DAL;
using System;
using System.Data.Entity;
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
            List<Project> ret = new List<Project>(7);
            using (var ctx = new StepMapDbContext())
            {
                var pl = ctx.Projects.Where(p => p.UserId == user.Id).Include(s => s.FinishedSteps).Include(s => s.User).ToList();
                for (int i = 0; i < 7; i++)
                {
                   ret.Add(pl.SingleOrDefault(p => p.Position == i));
                }
            }
            return ret;
        }

        public void AddProject(Project project)
        {
            var l = project.FinishedSteps.ToList();
            using (var ctx = new StepMapDbContext())
            {
                project = ctx.Projects.Add(project);
                ctx.SaveChanges();
            }
        }

        public void UpdateProject(Project project)
        {
            using (var ctx = new StepMapDbContext())
            {
                project = ctx.Projects.Attach(project);
                ctx.Entry(project).State = EntityState.Modified;
                ctx.Steps.AddRange(project.FinishedSteps.Where(s => s.Id == 0));
                ctx.SaveChanges();
            }
        }

        public void DeleteProject(int projectId)
        {
            using (var ctx = new StepMapDbContext())
            {
                var project = ctx.Projects.Include(s => s.FinishedSteps).SingleOrDefault(p => p.Id == projectId);
                if (project == null)
                {
                    throw new ArgumentException(string.Format("Project does not exsist: {0}.", projectId));
                }
                else
                {
                    ctx.Steps.RemoveRange(project.FinishedSteps);
                    ctx.Projects.Remove(project);
                    ctx.SaveChanges();
                }
            }
        }
    }
}
