using FactoryPunchList.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FactoryPunchList.App_Data;
using System.Net.Http;

namespace FactoryPunchList.Controllers
{
    public class AccountController : Controller
    {
        private MyDatabaseEntities db = new MyDatabaseEntities();
        private HttpClient client = new HttpClient();
        public ActionResult Signin()
        {
            SigninUserModel model = new SigninUserModel();
            return View(model);
        }

        public async Task<ActionResult> OAuth(string code)
        {
            var data = new Dictionary<string, string>
            {
                {"Content-Type", "application/x-www-form-urlencoded" },
                {"client_id", System.Configuration.ConfigurationManager.AppSettings.Get("ClientID") },
                {"client_secret", System.Configuration.ConfigurationManager.AppSettings.Get("ClientSecret")},
                {"grant_type", "authorization_code"},
                {"code", code },
                {"redirect_uri", "https://factorypunchlist.azurewebsites.net/Account/OAuth"}
            };

            var response = await client.PostAsync("https://developer.api.autodesk.com/authentication/v1/gettoken", new FormUrlEncodedContent(data));

            var content = response.Content.ReadAsStringAsync();

            if(response.IsSuccessStatusCode)
            {
                Guid guid = Guid.NewGuid();
                Coin coin = new Coin();
                coin.accessToken = code;
                coin.session = guid.ToString();
                db.Coins.Add(coin);
                db.SaveChanges();

                HttpCookie cook = new HttpCookie("UserIdentity");
                cook.Value = coin.session;
                Response.Cookies.Add(cook);

                return RedirectToAction("Index", "Punchlists");
            }

            SigninUserModel model = new SigninUserModel();
            return View(model);
        }

        public ActionResult Logout()
        {
            if (Request.Cookies["UserIdentity"] != null)
            {
                HttpCookie cook = Request.Cookies["UserIdentity"];
                cook.Expires = DateTime.Now.AddDays(-1);

                Response.Cookies.Add(cook);
            }


            return RedirectToAction("Signin");
        }
    }
}