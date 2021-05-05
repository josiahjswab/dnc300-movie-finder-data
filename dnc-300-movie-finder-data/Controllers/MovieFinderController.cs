using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using dnc_300_movie_finder_data.Models;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Hosting;
using dnc_300_movie_finder_data.Helpers;

namespace dnc_300_movie_finder_data.Controllers
{

    public class MovieFinderController : Controller
    {
        
        string filePath = HostingEnvironment.MapPath("~/dataa");

        private List<Movie> MoviesCacheTransferList = new List<Movie>();

        private DataSerializer dataSerializer = new DataSerializer();
        public ActionResult FindMovie()
        {
            return View();
        }

        //[HttpPost]
        public ActionResult MovieResults(string searchString)
        {
            List<Movie> MoviesCache = dataSerializer.BinaryDeserialize(filePath) as List<Movie>;
            if (MoviesCache != null)
            {
                foreach (var movie in MoviesCache)
                {
                    if (searchString == movie.Title)
                    {
                        return View(movie);
                    }
                    else
                    {
                        MoviesCacheTransferList.Add(movie);
                    }
                }
            }

            string response = CallOmdb($"http://www.omdbapi.com/?t={searchString}&apikey=af11b31");

            Movie dP = JsonConvert.DeserializeObject<Movie>(response);

            MoviesCacheTransferList.Add(dP);

            dataSerializer.BinarySerialize(MoviesCacheTransferList, filePath);

            List<Movie> foo = dataSerializer.BinaryDeserialize(filePath) as List<Movie>;

            return View(dP);
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
