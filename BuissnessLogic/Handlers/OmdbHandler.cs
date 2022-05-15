using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Movies;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BuissnessLogic.Handlers
{
    public class OmdbHandler
    {
        Uri RequestUri = new Uri(@"http://www.omdbapi.com/?apikey=694f1bcb");
        private HttpClient _client;

        public OmdbHandler(HttpClient client)
        {
            _client = client;
        }


        public async Task<PosterOnlyObject> GetPosterByIDAsync(string Id)
        {
            var url = RequestUri + $"&i=tt{Id}&plot=short";
            var responds = SendRequest(url);
            
            if(responds.IsSuccessStatusCode)
            {
                var content = await responds.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PosterOnlyObject>(content);
            }
            else
            {
                throw new Exception("No access too external API");
            }
        }

        public async Task<FullMovieDa> GetFullMovie(string id)
        {
            var url = RequestUri + $"&i=tt{id}&plot=full";
            var responds = SendRequest(url);

            if (responds.IsSuccessStatusCode)
            {
                var content = await responds.Content.ReadAsStringAsync();
               
                var movieDa = JsonConvert.DeserializeObject<FullMovieDa>(content);
                ManipulateDataAndAddLists(movieDa);
                return movieDa;
            }
            else
            {
                throw new Exception("No access too external API");
            }

        }

        private void ManipulateDataAndAddLists(FullMovieDa movieDa)
        {
            var actorsList = movieDa.Actors.Split(',').Select(p => p.Trim());
            List<string> list = new List<string>(actorsList);
            movieDa.ActorList = list;

            var directorsList = movieDa.Director.Split(',').Select(p => p.Trim());
            List<string> list2 = new List<string>(directorsList);
            movieDa.DirectorList = list2;
        }

        private HttpResponseMessage SendRequest(string content, string url)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };

            return _client.SendAsync(request).Result;
        }

        private HttpResponseMessage SendRequest(string url)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
            };

            return _client.SendAsync(request).Result;
        }

 

        public class PosterOnlyObject
        {
            public string Poster { get; set; }
            public string Plot { get; set; }
        }
    }
}


