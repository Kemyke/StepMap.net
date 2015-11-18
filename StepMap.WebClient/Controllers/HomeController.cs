using StepMap.ServiceContracts.DTO;
using StepMap.WebClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace StepMap.WebClient.Controllers
{
    public class HomeController : Controller
    {
        private static string ApiAddress = ConfigurationManager.AppSettings["ApiAddress"];
        private static string Resource = "/projects";
        private static string Address = ApiAddress + Resource;

        private UserStepMapViewModel GetProjects()
        {
            var client = CreateClient();
            var json = client.DownloadString(Address);
            var projects = System.Web.Helpers.Json.Decode<List<Project>>(json);

            var vm = new UserStepMapViewModel();
            vm.Projects = projects;
            return vm;
        }

        private System.Net.WebClient CreateClient()
        {
            var client = new System.Net.WebClient();
            client.Headers.Add("Content-Type", "application/json");
            client.Encoding = Encoding.UTF8;
            client.Headers.Add(HttpRequestHeader.Authorization, "Basic a2VteTphZG1pbg==");
            return client;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Stepmap";
            var vm = GetProjects();
            return View(vm);
        }

        public ActionResult SetProjectName(int projectId, string name)
        {
            var c = CreateClient();
            var x = c.DownloadString(Address);
            var z = System.Web.Helpers.Json.Decode<List<Project>>(x);
            var p = z.Single(tp => tp != null && tp.Id == projectId);

            var client = CreateClient();
            p.Name = name;
            string json = System.Web.Helpers.Json.Encode(p);
            client.UploadString(Address, "PUT", json);

            return View("Index", GetProjects());
        }

        public ActionResult CloseProject(int projectId)
        {
            var client = CreateClient();
            string json = System.Web.Helpers.Json.Encode(projectId);

            client.UploadString(Address, "DELETE", json);
            return View("Index", GetProjects());
        }

        public ActionResult CloseStep(int projectId)
        {
            var c = CreateClient();
            var x = c.DownloadString(Address);
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

            client.UploadString(Address, "PUT", json);
            return View("Index", GetProjects());
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

            client.UploadString(Address, "POST", json);
            return View("Index", GetProjects());
        }
    }
}
