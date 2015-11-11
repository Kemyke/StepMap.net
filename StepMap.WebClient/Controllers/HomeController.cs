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

        public ActionResult CreateProject(int position)
        {
            var client = new System.Net.WebClient();
            client.Headers.Add("Content-Type", "application/json");
            client.Encoding = Encoding.UTF8;
            client.Headers.Add(HttpRequestHeader.Authorization, "Basic a2VteTphZG1pbg==");

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
            return View();
        }
    }
}
