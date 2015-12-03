using StepMap.Common;
using StepMap.ServiceContracts;
using StepMap.ServiceContracts.DTO;
using StepMap.WebClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace StepMap.WebClient.Controllers
{
    public class AccountController : Controller
    {
        private static string ApiAddress = ConfigurationManager.AppSettings["ApiAddress"];
        private static string AccountResource = "/accounts";
        private static string ConfirmationResource = "/confirmation";

        private System.Net.WebClient CreateClient()
        {
            var client = new System.Net.WebClient();
            client.Headers.Add("Content-Type", "application/json");
            client.Encoding = Encoding.UTF8;
            client.Headers.Add(HttpRequestHeader.Authorization, WebApiApplication.AuthorizationHeader);
            return client;
        }

        public ActionResult Index()
        {
            return View();
        }

        private void CreateAuthorizationHeader(string username, string pwdHash)
        {
            string up = username + ":" + pwdHash;
            var b = System.Text.Encoding.UTF8.GetBytes(up);
            WebApiApplication.AuthorizationHeader = "Basic " + System.Convert.ToBase64String(b);
        }

        private IPrincipal CreatePrincipal(User user)
        {
            CustomPrincipal ret = new CustomPrincipal(user.Email);
            ret.Id = user.Id;
            ret.Name = user.Name;
            ret.Email = user.Email;
            return ret;
        }

        public ActionResult Login(FormCollection collection)
        {
            var pwdHash = CryptoHelper.CreatePasswordHash(collection["password"]);
            CreateAuthorizationHeader(collection["userName"], pwdHash);
            var client = CreateClient();

            var json = client.DownloadString(ApiAddress + AccountResource);
            var resp = System.Web.Helpers.Json.Decode<Response<User>>(json);

            if (resp.ResultCode == ResultCode.OK)
            {
                var p = CreatePrincipal(resp.Result);
                WebApiApplication.CurrentUser = p;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //TODO: Error handling
                return View("Index");
            }
        }

        public ActionResult SignUp()
        {
            return View();
        }

        public ActionResult Logout()
        {
            WebApiApplication.CurrentUser = null;
            return View("Index");
        }

        public ActionResult Register(FormCollection collection)
        {
            var client = CreateClient();
            var p = new { userName = collection["userName"], email = collection["email"], password = CryptoHelper.CreatePasswordHash(collection["password"]) };
            string json = System.Web.Helpers.Json.Encode(p);
            client.UploadString(ApiAddress + AccountResource, "POST", json);
            return View("Index");
        }

        public ActionResult ConfirmEmail(string guid)
        {
            var client = CreateClient();
            var json = client.DownloadString(ApiAddress + ConfirmationResource + "/" + guid);
            var resp = System.Web.Helpers.Json.Decode<Response<User>>(json);
            ConfirmEmailViewModel vm = new ConfirmEmailViewModel();
            if(resp.ResultCode == ResultCode.OK)
            {
                vm.Message = string.Format("hello {0}, your account is confirmed!", resp.Result.Name);
            }
            else
            {
                vm.Message = string.Format("Error during confirmation: {0}.", resp.ResultCode.ToString());
            }
            return View(vm);
        }
    }
}