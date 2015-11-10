using StepMap.WebClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
