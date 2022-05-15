using System.Net.Http;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;

namespace BuissnessLogic.Handlers
{
    public class TmdbHandler
    {
        Uri RequestUri =
            new Uri(
                @"https://api.themoviedb.org/3/search/person?api_key=bc2e8af508f762ff45464b05dcf68cbd&language=en-US&query=");

        string append =
            "&page=1&include_adult=false";

        private string imageurl = "https://image.tmdb.org/t/p/w200";

        private HttpClient _client;

        public TmdbHandler(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<FullPerson>> SearchPersonByName(string name)
        {
            var url = RequestUri + $"{name}&page=1&include_adult=false";
            var responds = SendRequest(url);

            if (responds.IsSuccessStatusCode)
            {
                var content = await responds.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<FullPerson>>(content);
            }
            else
            {
                throw new Exception("No access to external API");
            }
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

        
    }
}

public class FullPerson
{
    public string Name { get; set; }
    public string Id { get; set; }
    public string KnownForDepartment { get; set; }
    public string Popularity { get; set; }
    public string ProfilePath { get; set; }
    //public KnownFor KnownFor { get; set; }
}

public class KnownFor
{
    
}