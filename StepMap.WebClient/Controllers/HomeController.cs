using StepMap.Common;
using StepMap.Logger.Logging;
using StepMap.ServiceContracts;
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
    [CustomAuthorize]
    public class HomeController : Controller
    {
        private static string ApiAddress = ConfigurationManager.AppSettings["ApiAddress"];
        private static string Resource = "/projects";
        private static string Address = ApiAddress + Resource;
        private readonly ILogger logger;

        public HomeController()
        {
            logger = new Logger.Logging.Log4Net.Logger();
            logger.Debug("HomeController initialized!");
        }

        private UserStepMapViewModel GetProjects()
        {
            var client = CreateClient();
            logger.Debug("HttpRequestHeader.Authorization: {0}", client.Headers[HttpRequestHeader.Authorization]);
            var json = client.DownloadString(Address);
            var resp = System.Web.Helpers.Json.Decode<Response<IList<Project>>>(json);

            var vm = new UserStepMapViewModel();
            if (resp.ResultCode == ResultCode.OK)
            {
                vm.UserName = ((CustomPrincipal)WebApiApplication.CurrentUser).Name;
                vm.Projects = resp.Result;
            }
            else
            {
                //TODO: error handling
            }
            return vm;
        }

        private System.Net.WebClient CreateClient()
        {
            var client = new System.Net.WebClient();
            client.Headers.Add("Content-Type", "application/json");
            client.Encoding = Encoding.UTF8;
            client.Headers.Add(HttpRequestHeader.Authorization, WebApiApplication.AuthorizationHeader);//"Basic a2VteTphZG1pbg==");

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
            var z = System.Web.Helpers.Json.Decode<Response<IList<Project>>>(x).Result;
            var p = z.Single(tp => tp != null && tp.Id == projectId);

            var client = CreateClient();
            p.Name = name;
            string json = System.Web.Helpers.Json.Encode(p);

            //TODO: error handling
            client.UploadString(Address, "PUT", json);

            return View("Index", GetProjects());
        }

        public ActionResult CloseProject(int projectId)
        {
            try
            {
                var client = CreateClient();
                logger.Debug("HttpRequestHeader.Authorization: {0}", client.Headers[HttpRequestHeader.Authorization]);
                string json = System.Web.Helpers.Json.Encode(projectId);

                //TODO: error handling
                client.UploadString(Address, "DELETE", json);
                return View("Index", GetProjects());
            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString());
                return View("Error");
            }
        }

        public ActionResult CloseStep(int projectId)
        {
            try
            {
                var c = CreateClient();
                logger.Debug("HttpRequestHeader.Authorization: {0}", c.Headers[HttpRequestHeader.Authorization]);
                var x = c.DownloadString(Address);
                var z = System.Web.Helpers.Json.Decode<Response<IList<Project>>>(x).Result;
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

                //TODO: error handling
                client.UploadString(Address, "PUT", json);
                return View("Index", GetProjects());
            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString());
                return View("Error");
            }
        }

        public ActionResult CreateProject(int position)
        {
            var client = CreateClient();
            logger.Debug("HttpRequestHeader.Authorization: {0}", client.Headers[HttpRequestHeader.Authorization]);

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

            //TODO: error handling
            client.UploadString(Address, "POST", json);
            return View("Index", GetProjects());
        }
    }
}
