using BuissnessLogic.Handlers;
using DataAccess.Comments;
using DataAccess.Factories;
using DataAccess.Movies;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace SEP6_Backend_WebAPI.Controllers
{
    [Route("api/v1/movie")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieHandler _movieHandler;
        private readonly OmdbHandler _obmHandler;
        private readonly TmdbHandler _tmdbHandler;

        public MovieController(IDaFactory daFactory)
        {
            _movieHandler = new MovieHandler(daFactory.MoviesRepository());
            _obmHandler = new OmdbHandler(new System.Net.Http.HttpClient());
            _tmdbHandler = new TmdbHandler(new System.Net.Http.HttpClient());
        }

        [HttpGet]
        [Route("title/{title}")]
        public MovieDa GetByTitle([FromRoute] string title)
        {
            return _movieHandler.GetMovieByTitle(title);
        }

        [HttpGet]
        [Route("full/{id}")]
        public FullMovieDa GetFullMovie([FromRoute] string id)
        {
            return _obmHandler.GetFullMovie(id).Result;
        }

        [HttpGet]
        [Route("titleandposter/{title}")]
        public List<MovieDa> GetAllByTitle([FromRoute] string title)
        {
            var MovieList = _movieHandler.GetMoviesByTitle(title);

            foreach (var Movie in MovieList)
            {
                try
                {
                    var obmObj = _obmHandler.GetPosterByIDAsync(Movie.Id);
                    Movie.PosterHttp = obmObj.Result.Poster;
                    Movie.Plot = obmObj.Result.Plot;

                    if (Movie.PosterHttp == "N/A")
                        Movie.PosterHttp = @"..\assets\not-found-image-15383864787lu.jpg";
                }
                catch (Exception e)
                {
                    //return StatusCode(150);
                }
            }

            return MovieList;
        }


        [HttpGet]
        [Route("checkFavorite/{email}/{id}")]
        public FavoriteMovieModel CheckFavorite([FromRoute] string email, [FromRoute] string id)
        {
            FavoriteMovieModel model = _movieHandler.CheckFavorite(email, id);
            return model;
        }

        [HttpGet]
        [Route("discovery/popularity")]
        public TmdbMovie.Root GetMostPopularMovies()
        {
            var MovieList = _tmdbHandler.GetMostPopularMovies().Result;
            return MovieList;
        }

        [HttpGet]
        [Route("getFavorites/{userProfile}")]
        public List<FavoriteMovieModel> GetAllFavorites([FromRoute] string userProfile)
        {
            List<FavoriteMovieModel> MovieList = _movieHandler.GetAllFavorites(userProfile);
            return MovieList;
        }

        [HttpGet]
        [Route("discovery/upcomming")]
        public TmdbMovie.Root GetUpcomingMovies()
        {
            var MovieList = _tmdbHandler.GetUpcommingMovies().Result;
            return MovieList;
        }

        [HttpGet]
        [Route("discovery/theaters")]
        public TmdbMovie.Root GetInTheatersMovies()
        {
            var MovieList = _tmdbHandler.GetInTheathersMovies().Result;
            return MovieList;
        }


        [HttpGet]
        [Route("discovery/seriespopular")]
        public RootSeries GetMostPopularSeries()
        {
            var MovieList = _tmdbHandler.GetMostPopularSeries().Result;
            return MovieList;
        }

        [HttpPost]
        [Route("setFavorite")]
        public FavoriteMovieModel SetFavorite(FavoriteMovieModel model)
        {
           return _movieHandler.SetFavorite(model);
            //test webhook
        }
    }
}