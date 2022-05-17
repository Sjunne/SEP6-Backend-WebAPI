using System.Net.Http;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DataAccess.Actors;
using Newtonsoft.Json.Linq;

namespace BuissnessLogic.Handlers
{
    public class TmdbHandler
    {
        Uri RequestUri =
            new Uri(
                @"https://api.themoviedb.org/3/search/person?api_key=bc2e8af508f762ff45464b05dcf68cbd&language=en-US&query=");

        public Uri Person = new Uri(@"https://api.themoviedb.org/3/person/");
        public string PersonId = @"?api_key=bc2e8af508f762ff45464b05dcf68cbd&language=en-US";

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
                var seriesCollection = JObject.Parse(content)["results"]
                    .ToObject<List<FullPerson>>();
                var result = TransformPersonList(seriesCollection);
                return result;
                
               // return JsonConvert.DeserializeObject<List<ResponseSearchPeople>>(content);

            }
            else
            {
                throw new Exception("No access to external API");
            }
        }
        
        public async Task<PersonDetail> SearchPersonById(string id)
        {
            var url = Person + $"{id}" + PersonId;
            var responds = SendRequest(url);

            if (responds.IsSuccessStatusCode)
            {
                var content = await responds.Content.ReadAsStringAsync();
                var result =  JsonConvert.DeserializeObject<PersonDetail>(content);
                var r = TransformPersonDetail(result);
                return r;
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
        
        public List<FullPerson> TransformPersonList(List<FullPerson> list)
       {
           foreach (var person in list)
           {
               person.profile_path = "https://image.tmdb.org/t/p/w200" + person.profile_path;
           }

           return list;
       }
        public PersonDetail TransformPersonDetail(PersonDetail pd)
        {
            pd.profile_path = "https://image.tmdb.org/t/p/w200" + pd.profile_path;
            var results = SearchPersonByName(pd.name);
            foreach (var person in results.Result)
            {
                if (person.name == pd.name && person.id == pd.id)
                {
                    pd.known_for = person.known_for;
                } 
            }
            return pd;
        }
    }
}

public class PersonDetail
{
    public bool adult { get; set; }
    public List<string> also_known_as { get; set; }
    public string biography { get; set; }
    public string birthday { get; set; }
    public object deathday { get; set; }
    public int gender { get; set; }
    public string homepage { get; set; }
    public int id { get; set; }
    public string imdb_id { get; set; }
    public string known_for_department { get; set; }
    public string name { get; set; }
    public string place_of_birth { get; set; }
    public double popularity { get; set; }
    public string profile_path { get; set; }
    public List<KnownFor> known_for { get; set; }

}

public class KnownFor
{
    public string poster_path { get; set; }
    public bool adult { get; set; }
    public string overview { get; set; }
    public string release_date { get; set; }
    public string original_title { get; set; }
    public List<object> genre_ids { get; set; }
    public int id { get; set; }
    public string media_type { get; set; }
    public string original_language { get; set; }
    public string title { get; set; }
    public string backdrop_path { get; set; }
    public double popularity { get; set; }
    public int vote_count { get; set; }
    public bool video { get; set; }
    public double vote_average { get; set; }
    public string first_air_date { get; set; }
    public List<string> origin_country { get; set; }
    public string name { get; set; }
    public string original_name { get; set; }
}
  
public class FullPerson
{
    public string profile_path { get; set; }
    public bool adult { get; set; }
    public int id { get; set; }
    public List<KnownFor> known_for { get; set; }
    public string name { get; set; }
    public double popularity { get; set; }
}
  
public class ResponseSearchPeople
{
    public int page { get; set; }
    public List<FullPerson> results { get; set; }
    public int total_results { get; set; }
    public int total_pages { get; set; }
}
  
public class ResponsePerson
{
    public bool adult { get; set; }
    public List<string> also_known_as { get; set; }
    public string biography { get; set; }
    public string birthday { get; set; }
    public string deathday { get; set; }
    public int gender { get; set; }
    public string homepage { get; set; }
    public int id { get; set; }
    public string imdb_id { get; set; }
    public string name { get; set; }
    public string place_of_birth { get; set; }
    public double popularity { get; set; }
    public string profile_path { get; set; }
}
public class PagingInfo
{
    public int TotalItems { get; set; }
    public int ItemsPerPage { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages
    {
        get
        {
            return (int)Math.Ceiling((decimal)TotalItems /
                ItemsPerPage);
        }
    }
}