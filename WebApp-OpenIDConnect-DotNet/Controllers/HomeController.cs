using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebApp_OpenIDConnect_DotNet.Controllers
{

    public class HomeController : Controller
    {
        public string GetEnvironmentUri()
        {
            string myJson = "{'name': '" + (User.Identity.Name).ToLower() + "'}";

            // Call Azure Functions App to decide which environment user should be sent to

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(10);
                try
                {
                    var response = client.PostAsync(
                                            "https://selfhostredirectfunction.azurewebsites.net/api/environmentInfo",
                                             new StringContent(myJson, Encoding.UTF8, "application/json")).Result;

                    var test = response.Content.ReadAsStringAsync().Result;
                    return (JsonConvert.DeserializeObject(test).ToString());
                    
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        public ActionResult Index()
        {
            if (User.Identity.Name == "")
            {
                return View();
            }
            else
            {
                string redirectUri = GetEnvironmentUri();
                if (redirectUri != "")
                {
                    return Redirect(redirectUri);
                }

                return View();
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}