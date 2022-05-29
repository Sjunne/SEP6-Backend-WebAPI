using DataAccess.Movies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuissnessLogic.Handlers
{
    public interface ITmdbHandler
    {
        Task<List<FullPerson>> SearchPersonByName(string name);
        Task<PersonDetail> SearchPersonById(string id);
        List<FullPerson> TransformPersonList(List<FullPerson> list);
        PersonDetail TransformPersonDetail(PersonDetail pd);
        Task<ExternalIds> GetImdbId(int id);
        Task<TmdbMovie.Root> GetMostPopularMovies();
        Task<TmdbMovie.Root> GetUpcommingMovies();
        Task<ExternalIds> GetImdbIdForSeries(int id);
        Task<TmdbMovie.Root> GetInTheathersMovies();
        Task<RootSeries> GetMostPopularSeries();
        string GetSeriesImdb(int id);
        List<Cast> GetFullCreditAsCast(string id);
        Task<CombinedCredits> GetFullCredits(string id);
        Task<List<FullPerson>> GetPopularActors();
        List<FullPerson> TransformPopularActorsList(List<FullPerson> list);
        List<Crew> GetFullCreditAsCrew(string id);
        string GetTransformedImdb(int id);

    }
}