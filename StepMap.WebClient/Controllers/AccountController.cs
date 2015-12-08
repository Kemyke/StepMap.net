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
using System.Web.Script.Serialization;
using System.Web.Security;

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
            return View();
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
            var userName = collection["userName"];
            var pwdHash = CryptoHelper.CreatePasswordHash(collection["password"]);
            
            var client = CreateClient();
            string up = userName + ":" + pwdHash;
            var b = System.Text.Encoding.UTF8.GetBytes(up);
            var ah = "Basic " + System.Convert.ToBase64String(b); 
            client.Headers.Add(HttpRequestHeader.Authorization, ah);

            var json = client.DownloadString(ApiAddress + AccountResource);
            var resp = System.Web.Helpers.Json.Decode<Response<User>>(json);

            if (resp.ResultCode == ResultCode.OK)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                CustomPrincipalSer serializeModel = new CustomPrincipalSer()
                {
                    Id = resp.Result.Id,
                    Name = resp.Result.Name,
                    Email = resp.Result.Email,
                    Hash = pwdHash,
                };
                string userData = serializer.Serialize(serializeModel);

                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                         1,
                         resp.Result.Name,
                         DateTime.Now,
                         DateTime.Now.AddMinutes(15),
                         false,
                         userData);
                
                string encTicket = FormsAuthentication.Encrypt(authTicket);
                HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                Response.Cookies.Add(faCookie);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                //TODO: Error handling
                return View("Index", new LoginViewModel() { ErrorMessage = string.Format("Error: {0}.", resp.ResultCode.ToString()) });
            }
        }

        public ActionResult SignUp()
        {
            return View();
        }

        public ActionResult Logout()
        {
            System.Web.HttpContext.Current.User = null;
            HttpCookie c = Request.Cookies[FormsAuthentication.FormsCookieName];
            c.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Set(c);
            Session.Clear();

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