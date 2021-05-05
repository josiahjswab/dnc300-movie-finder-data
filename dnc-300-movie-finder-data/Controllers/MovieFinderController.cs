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
        // Creates a file "dataa" that is seralized binary.
        string filePath = HostingEnvironment.MapPath("~/dataa");
        // We need a new list obj to carry over items from previous list or to substantiate a new one.
        private List<Movie> MoviesCacheTransferList = new List<Movie>();
        // Calling our helper class.
        private DataSerializer dataSerializer = new DataSerializer();
        public ActionResult FindMovie()
        {
            return View();
        }

        //[HttpPost]
        public ActionResult MovieResults(string searchString)
        {
            //Pull in file and turn it into a list of movies then loop titles if one matches return view with data. Else we transfer the movies to a new array.
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
            //API call after determining the query wasn't in the cache.
            string response = CallOmdb($"http://www.omdbapi.com/?t={searchString}&apikey=af11b31");
            // Turn string into obj.
            Movie dP = JsonConvert.DeserializeObject<Movie>(response);
            //Adding our new search data to the list.
            MoviesCacheTransferList.Add(dP);
            //Turning the list back into serialized binary.
            dataSerializer.BinarySerialize(MoviesCacheTransferList, filePath);
            // This read was to step through with the debug tool to ensure that the write file worked.
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
