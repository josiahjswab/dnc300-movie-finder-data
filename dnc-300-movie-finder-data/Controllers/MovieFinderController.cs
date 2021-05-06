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

        public ActionResult FindMovie()
        {
            return View();
        }
        

        //private readonly DataSerializer _dataSerializer = new DataSerializer();

        readonly string filePath = HostingEnvironment.MapPath("~/movie_list");

        private readonly List<Movie> _moviesCacheTransferList = new List<Movie>();
        public ActionResult MovieResults(string searchString)
        {

            List<Movie> moviesCache = DataSerializer.BinaryDeserialize(filePath) as List<Movie>;

            if (moviesCache != null)
            {
                foreach (var movie in moviesCache)
                {
                    //TODO: Handle comparison without regard to caps.
                    if (searchString == movie.Title)
                    {
                        return View(movie);
                    }
                    _moviesCacheTransferList.Add(movie);
                }
            }
            
            string response = HitApi.Url($"http://www.omdbapi.com/?t={searchString}&apikey=af11b31");

            Movie dP = JsonConvert.DeserializeObject<Movie>(response);

            _moviesCacheTransferList.Add(dP);

            DataSerializer.BinarySerialize(_moviesCacheTransferList, filePath);

            return View(dP);
        }
    }

}
