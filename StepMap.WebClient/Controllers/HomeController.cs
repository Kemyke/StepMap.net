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
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;

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
                vm.UserName = ((CustomPrincipal)System.Web.HttpContext.Current.User).Name;
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

            var user = System.Web.HttpContext.Current.User as CustomPrincipal;
            if (user != null)
            {
                string up = user.Name + ":" + user.Hash;
                var b = System.Text.Encoding.UTF8.GetBytes(up);
                var ah = "Basic " + System.Convert.ToBase64String(b); 
                client.Headers.Add(HttpRequestHeader.Authorization, ah);
            }
            return client;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Stepmap";
            try
            {
                var vm = GetProjects();
                return View(vm);
            }
            catch(WebException ex)
            {
                var resp = ex.Response as HttpWebResponse;
                if(resp.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
                    return RedirectToAction("Index", "Account");
                }
                throw;
            }
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

        public ActionResult SetStepName(int stepId, string name)
        {
            var c = CreateClient();
            var x = c.DownloadString(Address);
            var z = System.Web.Helpers.Json.Decode<Response<IList<Project>>>(x).Result;
            var p = z.Single(tp => tp != null && tp.FinishedSteps.Any(s => s.Id == stepId));

            var client = CreateClient();
            var step = p.FinishedSteps.Single(s => s.Id == stepId);
            step.Name = name;
            string json = System.Web.Helpers.Json.Encode(p);

            //TODO: error handling
            client.UploadString(Address, "PUT", json);

            return View("Index", GetProjects());
        }

        public ActionResult SetStepDeadline(int stepId, string deadline)
        {
            var c = CreateClient();
            var x = c.DownloadString(Address);
            var z = System.Web.Helpers.Json.Decode<Response<IList<Project>>>(x).Result;
            var p = z.Single(tp => tp != null && tp.FinishedSteps.Any(s => s.Id == stepId));

            var client = CreateClient();
            var step = p.FinishedSteps.Single(s => s.Id == stepId);
            step.Deadline = DateTime.Parse(deadline);
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
