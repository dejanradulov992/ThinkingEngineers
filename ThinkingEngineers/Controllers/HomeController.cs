using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Web;
using System.Web.Mvc;
using ThinkingEngineers.Models;

namespace ThinkingEngineers.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            IEnumerable<Book> books = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://te-testapp.azurewebsites.net/api/");

                const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
                const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
                ServicePointManager.SecurityProtocol = Tls12;
                //HTTP GET
                var responseTask = client.GetAsync("library");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var responseBody = result.Content.ReadAsStringAsync();

                    //var readTask = result.Content.ReadAsAsync<IList<Book>>();
                    
                    //readTask.Wait();

                    //books = responseBody.Result;

                    Result ObjectName = JsonConvert.DeserializeObject<Result>(responseBody.Result);
                }
                else //web api sent error response 
                {
                    //log response status here..

                    books = Enumerable.Empty<Book>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(books);
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