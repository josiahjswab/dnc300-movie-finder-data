using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using dnc_300_movie_finder_data.Models;

namespace dnc_300_movie_finder_data.Controllers
{
    public class MovieFinderController : Controller

    {
        public ActionResult FindMovie()
        {
            Console.WriteLine("Something");
            //var movie = new Movie() { Title = "101 dalmations" };
            return View();
        }

        //[HttpPost]
        public ActionResult MovieResults(string searchString)
        {
            string response = CallOmdb($"http://www.omdbapi.com/?t={searchString}&apikey=af11b31");
            return Content(response);
        }

        public static string CallOmdb(string url)
        {
            var client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            var response = client.DownloadString(url);
            return response;
        }

    }

}
