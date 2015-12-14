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
        private readonly INotificationManager notificationManager;

        public ProjectManager(INotificationManager notificationManager)
        {
            this.notificationManager = notificationManager;
        }

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
                foreach(var step in ctx.Steps.Where(s=>s.Id != 0))
                {
                    var s = ctx.Steps.Attach(step);
                    ctx.Entry(s).State = EntityState.Modified;
                }
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

        private void SentFirstReminder(User user, Project project, Step step)
        {
            using (var ctx = new StepMapDbContext())
            {
                //TODO: Config, customize, randomize
                Reminder reminder = new Reminder()
                {
                    EmailAddress = user.Email,
                    Message = string.Format("Your current step ({0}) in project {1} is delayed! Get yourself together!", step.Name, project.Name), //LOCSTR
                    Subject = "First reminder", //LOCSTR
                    SentDate = DateTime.UtcNow,
                    StepId = step.Id
                };
                ctx.Reminders.Add(reminder);
                notificationManager.SendEmail(user, reminder.Subject, reminder.Message);
                ctx.SaveChanges();
            }
        }

        public void CheckProjectProgress(Project project)
        {
            using (var ctx = new StepMapDbContext())
            {
                Step currentStep = project.FinishedSteps.Last();
                if (currentStep.Deadline < DateTime.UtcNow)
                {
                    project.BadPoint++;
                    if (!currentStep.SentReminders.Any())
                    {
                        SentFirstReminder(project.User, project, currentStep);
                        ctx.Projects.Attach(project);
                    }
                }

                ctx.SaveChanges();
            }
        }
    }
}
