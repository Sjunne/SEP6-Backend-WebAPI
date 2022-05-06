using BuissnessLogic.Handlers;
using DataAccess.Factories;
using DataAccess.Movies;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace SEP6_Backend_WebAPI.Controllers
{
    [Route("api/v1/movie")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieHandler _movieHandler;

        public MovieController(IDaFactory daFactory)
        {
            _movieHandler = new MovieHandler(daFactory.MoviesRepository());
        }

        [HttpGet]
        [Route("title/{title}")]
        public MovieDa GetByTitle([FromRoute]string title)
        {
            return _movieHandler.GetMovieByTitle(title);
        }
    }
}
