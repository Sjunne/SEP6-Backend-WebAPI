using System.Net.Http;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using DataAccess.Actors;
using Newtonsoft.Json.Linq;
using DataAccess.Movies;

namespace BuissnessLogic.Handlers
{
    public class TmdbHandler : ITmdbHandler
    {
        Uri credits = new Uri(@"https://api.themoviedb.org/3/person/");
        Uri popularActors = new Uri(@"https://api.themoviedb.org/3/person/popular?api_key=bc2e8af508f762ff45464b05dcf68cbd&language=en-US&page=1");

        Uri RequestUri =
            new Uri(
                @"https://api.themoviedb.org/3/search/person?api_key=bc2e8af508f762ff45464b05dcf68cbd&language=en-US&query=");

        public Uri Person = new Uri(@"https://api.themoviedb.org/3/person/");

        private readonly Uri discoverUri =
            new Uri(
                @"https://api.themoviedb.org/3/discover/movie?api_key=bc2e8af508f762ff45464b05dcf68cbd&language=en-US");

        string append =
            "&page=1&include_adult=false";

        private Uri i = new Uri(@"https://api.themoviedb.org/3/movie/");

        private string PersonId = "?api_key=bc2e8af508f762ff45464b05dcf68cbd&language=en-US";

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
                var result = JsonConvert.DeserializeObject<PersonDetail>(content);
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

            foreach (var movie in pd.known_for)
            {
                string[] words = GetImdbId(movie.id).Result.imdb_id.Split('t');
                movie.imdb_id = words[2];
            }

            pd.known_for = OrderByYear(pd.known_for);
            return pd;
        }

        private List<KnownFor> OrderByYear(List<KnownFor> pdKnownFor)
        {
            foreach (var movie in pdKnownFor)
            {
                movie.original_title = movie.original_title + " (" + movie.release_date.Split("-")[0] + ")";
            }

            pdKnownFor = pdKnownFor.OrderBy(x => x.release_date).ToList();
            return pdKnownFor;
        }

        public async Task<ExternalIds> GetImdbId(int id)
        {
            var url = i + $"{id}" + "/external_ids?api_key=bc2e8af508f762ff45464b05dcf68cbd";
            var responds = SendRequest(url);
            if (responds.IsSuccessStatusCode)
            {
                var content = await responds.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ExternalIds>(content);
            }
            else
            {
                throw new Exception("No access to external API");
            }
        }

        public async Task<TmdbMovie.Root> GetMostPopularMovies()
        {
            var url = discoverUri +
                      "&sort_by=popularity.desc&include_adult=false&include_video=false&page=1&with_watch_monetization_types=flatrate";
            var responds = SendRequest(url);
            if (responds.IsSuccessStatusCode)
            {
                var content = await responds.Content.ReadAsStringAsync();
                var root = JsonConvert.DeserializeObject<TmdbMovie.Root>(content);
                foreach (var movie in root.results)
                {
                    movie.imdb_id = GetTransformedImdb(movie.id);
                }
                return root;
            }
            else
            {
                throw new Exception("No access to external API");
            }
        }

        public async Task<TmdbMovie.Root> GetUpcommingMovies()
        {
            var url = "https://api.themoviedb.org/3/movie/upcoming?api_key=bc2e8af508f762ff45464b05dcf68cbd&language=en-US&page=1";
            var responds = SendRequest(url);
            if (responds.IsSuccessStatusCode)
            {
                var content = await responds.Content.ReadAsStringAsync();
                var root = JsonConvert.DeserializeObject<TmdbMovie.Root>(content);
                foreach (var movie in root.results)
                {
                    movie.imdb_id = GetTransformedImdb(movie.id);
                }
                return root;
            }
            else
            {
                throw new Exception("No access to external API");
            }
        }
        
        public async Task<ExternalIds> GetImdbIdForSeries(int id)
        {
            var url = i + $"{id}" + "/external_ids?api_key=bc2e8af508f762ff45464b05dcf68cbd";
            var responds = SendRequest(url);
            if (responds.IsSuccessStatusCode)
            {
                var content = await responds.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ExternalIds>(content);
            }
            else
            {
                return new ExternalIds();
            }
        }

        public async Task<TmdbMovie.Root> GetInTheathersMovies()
        {
            var url = "https://api.themoviedb.org/3/movie/now_playing?api_key=bc2e8af508f762ff45464b05dcf68cbd&language=en-US&page=1";
            var responds = SendRequest(url);
            if (responds.IsSuccessStatusCode)
            {
                var content = await responds.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TmdbMovie.Root>(content);
            }
            else
            {
                throw new Exception("No access to external API");
            }
        }

        public async Task<RootSeries> GetMostPopularSeries()
        {
            var url = "https://api.themoviedb.org/3/tv/popular?api_key=bc2e8af508f762ff45464b05dcf68cbd&language=en-US&page=1";
            var responds = SendRequest(url);
            if (responds.IsSuccessStatusCode)
            {
                var content = await responds.Content.ReadAsStringAsync();
                var root =  JsonConvert.DeserializeObject<RootSeries>(content);
                List<TmdbSeries> series = new List<TmdbSeries>();
                foreach (var serie in root.results)
                {
                    serie.imdb_db = GetSeriesImdb(serie.id);
                    if (serie.imdb_db != "")
                    {
                        series.Add(serie);
                    }
                }

                root.results = series;
                return root;
            }
            else
            {
                throw new Exception("No access to external API");
            }
        }
        
        public string GetSeriesImdb(int id)
        {
            var imdb_id = "";
            var imdb = GetImdbIdForSeries(id);
            if (imdb.Result != null && imdb.Result.imdb_id != null && imdb.Result.imdb_id != "")
            {
                imdb_id = imdb.Result.imdb_id.Split("t")[2];
            }

            return imdb_id;
        }

        //
        public List<Cast> GetFullCreditAsCast(string id)
        {
            var combined = GetFullCredits(id).Result;
            var c =  CastOrderByYear(combined.cast);
            return c;
        }
        
        private List<Cast> CastOrderByYear(List<Cast> cast)
        {
            List<Cast> c = new List<Cast>();
            for (int j = 0; j < cast.Count; j++)
            {
                if (!string.IsNullOrEmpty(cast[j].release_date) && cast[j].vote_average != 0)
                {
                    Console.WriteLine(cast[j].original_title);
                    var movie = cast[j];
                    cast[j].title = movie.original_title + " (" + movie.release_date.Split("-")[0] + ")";
                    cast[j].imdb_id = GetTransformedImdb(cast[j].id);
                    c.Add(cast[j]);
                }
            }
            double sum = 0.0;

            foreach (var movie in c)
            {
                var imdb = GetImdbId(movie.id).Result.imdb_id;
                if (imdb != null)
                {
                    sum += movie.vote_average;
                    movie.imdb_id = imdb.Split("t")[2];
                    
                }
            }
            var ca = c.OrderBy(x => x.release_date).ToList();
            var median = sum / c.Count;
            ca[0].median = Math.Round(median, 1);
            return ca;
        }
        
        private List<Crew> CrewOrderByYear(List<Crew> crew)
        {
            List<Crew> c = new List<Crew>();
            for (int j = 0; j < crew.Count; j++)
            {
                if (!string.IsNullOrEmpty(crew[j].release_date) && !string.IsNullOrEmpty(crew[j].poster_path))
                {
                    var movie = crew[j];
                    crew[j].title = movie.original_title + " (" + movie.release_date.Split("-")[0] + ")";
                    crew[j].imdb_id = GetTransformedImdb(crew[j].id);
                    c.Add(crew[j]);
                }
            }
            double sum = 0.0;

            foreach (var movie in c)
            {
                var imdb = GetImdbId(movie.id).Result.imdb_id;
                if (imdb != null)
                {
                    sum += movie.vote_average;
                    movie.imdb_id = imdb.Split("t")[2];
                    
                }
            }
            var ca = c.OrderBy(x => x.release_date).ToList();
            var median = sum / c.Count;
            ca[0].median = Math.Round(median, 1);
            return ca;
        }

        public async Task<CombinedCredits> GetFullCredits(string id)
        {
            var url = credits + $"{id}" +
                      "?api_key=bc2e8af508f762ff45464b05dcf68cbd&language=en-US&append_to_response=combined_credits";
            var responds = SendRequest(url);
            if (responds.IsSuccessStatusCode)
            {
                var content = await responds.Content.ReadAsStringAsync();
                var seriesCollection = JObject.Parse(content)["combined_credits"]
                    .ToObject<CombinedCredits>();
                return seriesCollection;
            }
            else
            {
                throw new Exception("No access to external API");
            }
        }

        public async Task<List<FullPerson>> GetPopularActors()
        {
            var responds = SendRequest(popularActors.ToString());

            if (responds.IsSuccessStatusCode)
            {
                var content = await responds.Content.ReadAsStringAsync();
                var seriesCollection = JObject.Parse(content)["results"]
                    .ToObject<List<FullPerson>>();
                var result = TransformPopularActorsList(seriesCollection);
                return result;
            }
            else
            {
                throw new Exception("No access to external API");
            }
        }
        
        public List<FullPerson> TransformPopularActorsList(List<FullPerson> list)
        {
            List<FullPerson> l = new List<FullPerson>();
            foreach (var person in list)
            {
                if (person.profile_path != null)
                {
                    person.profile_path = "https://image.tmdb.org/t/p/w200" + person.profile_path;
                    l.Add(person);
                }
               
            }

            return l;
        }

        public List<Crew> GetFullCreditAsCrew(string id)
        {
            var combined = GetFullCredits(id).Result;
            var c =  CrewOrderByYear(combined.crew);
            return c;
        }

        public string GetTransformedImdb(int id)
        {
            var imdb_id = "";
            var imdb = GetImdbId(id).Result.imdb_id;
            if (imdb != null)
            {
                imdb_id = imdb.Split("t")[2];
            }

            return imdb_id;
        }
    }
}

public class Cast
{
    public bool adult { get; set; }
    public string backdrop_path { get; set; }
    public List<int> genre_ids { get; set; }
    public int vote_count { get; set; }
    public string original_language { get; set; }
    public string original_title { get; set; }
    public string poster_path { get; set; }
    public double vote_average { get; set; }
    public bool video { get; set; }
    public int id { get; set; }
    public string title { get; set; }
    public string overview { get; set; }
    public string release_date { get; set; }
    public double popularity { get; set; }
    public string character { get; set; }
    public string credit_id { get; set; }
    public int order { get; set; }
    public string media_type { get; set; }
    public string original_name { get; set; }
    public List<string> origin_country { get; set; }
    public string name { get; set; }
    public string first_air_date { get; set; }
    public int? episode_count { get; set; }
    public string imdb_id { get; set; }
    public double median { get; set; }
}

public class CombinedCredits
{
    public List<Cast> cast { get; set; }
    public List<Crew> crew { get; set; }
}

public class Root
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
    public CombinedCredits combined_credits { get; set; }
}

public class Crew
{
    public bool adult { get; set; }
    public string backdrop_path { get; set; }
    public List<int> genre_ids { get; set; }
    public int id { get; set; }
    public string original_language { get; set; }
    public string original_title { get; set; }
    public string overview { get; set; }
    public string poster_path { get; set; }
    public string release_date { get; set; }
    public string title { get; set; }
    public bool video { get; set; }
    public double vote_average { get; set; }
    public int vote_count { get; set; }
    public double popularity { get; set; }
    public string credit_id { get; set; }
    public string department { get; set; }
    public string job { get; set; }
    public string media_type { get; set; }
    public string original_name { get; set; }
    public List<string> origin_country { get; set; }
    public string name { get; set; }
    public int? episode_count { get; set; }
    public string imdb_id { get; set; }
    public double median { get; set; }
}

public class ExternalIds
{
    public int id { get; set; }
    public string imdb_id { get; set; }
    public string facebook_id { get; set; }
    public string instagram_id { get; set; }
    public string twitter_id { get; set; }
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

    //public CombinedCredits combined_credits { get; set; }
    public List<Cast> cast { get; set; }
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
    public string imdb_id { get; set; }
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
            return (int) Math.Ceiling((decimal) TotalItems /
                                      ItemsPerPage);
        }
    }
}