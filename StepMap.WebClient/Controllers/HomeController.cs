using StepMap.ServiceContracts.DTO;
using StepMap.WebClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace StepMap.WebClient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Stepmap";
            var vm = new UserStepMapViewModel { Projects = new List<ServiceContracts.DTO.Project>() };
            for (int i = 0; i < 7; i++)
            {
                vm.Projects.Add(null);
            }
            return View(vm);
        }

        private System.Net.WebClient CreateClient()
        {
            var client = new System.Net.WebClient();
            client.Headers.Add("Content-Type", "application/json");
            client.Encoding = Encoding.UTF8;
            client.Headers.Add(HttpRequestHeader.Authorization, "Basic a2VteTphZG1pbg==");
            return client;
        }

        public ActionResult CloseProject(int projectId)
        {
            var client = CreateClient();
            string json = System.Web.Helpers.Json.Encode(projectId);

            client.UploadString("http://localhost:55300/Service.svc/projects", "DELETE", json);
            return View("Index", new UserStepMapViewModel { Projects = new List<ServiceContracts.DTO.Project>() });
        }

        public ActionResult CloseStep(int projectId)
        {
            var c = CreateClient();
            var x = c.DownloadString("http://localhost:55300/Service.svc/projects");
            var z = System.Web.Helpers.Json.Decode<List<Project>>(x);
            var p = z.Single(tp => tp != null && tp.Id == projectId);

            var client = CreateClient();
            p.FinishedSteps.Last().FinishDate = DateTime.Now;
            p.FinishedSteps.Add(new Step()
                {
                    ProjectId = p.Id,
                    Name = "Next step",
                    Deadline = DateTime.Now,
                });

            string json = System.Web.Helpers.Json.Encode(p);

            client.UploadString("http://localhost:55300/Service.svc/projects", "PUT", json);
            return View("Index", new UserStepMapViewModel { Projects = new List<ServiceContracts.DTO.Project>() });
        }

        public ActionResult CreateProject(int position)
        {
            var client = CreateClient();

            Project p = new Project()
            {
                Name = "New project",
                StartDate = DateTime.Now,
                GoodPoint = 0,
                BadPoint = 0,
                Position = position,
                FinishedSteps = new List<Step>() { new Step() { Name = "First step", Deadline = DateTime.Now, FinishDate = null, SentReminder = 0 } }
            };
            string json = System.Web.Helpers.Json.Encode(p);

            client.UploadString("http://localhost:55300/Service.svc/projects", "POST", json);
            return View("Index", new UserStepMapViewModel { Projects = new List<ServiceContracts.DTO.Project>() });
        }
    }
}
