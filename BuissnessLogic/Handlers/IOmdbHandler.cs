using static BuissnessLogic.Handlers.OmdbHandler;
using System.Threading.Tasks;
using DataAccess.Movies;
using System.Net.Http;

namespace BuissnessLogic.Handlers
{
    public interface IOmdbHandler
    {
        Task<PosterOnlyObject> GetPosterByIDAsync(string Id);
        Task<FullMovieDa> GetFullMovie(string id);
    }
}