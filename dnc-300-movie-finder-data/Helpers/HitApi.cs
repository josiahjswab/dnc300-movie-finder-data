using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace dnc_300_movie_finder_data.Helpers
{
    public class HitApi
    {
        public static string Url(string url)
        {
            var client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            var response = client.DownloadString(url);
            return response;
        }
    }
}
