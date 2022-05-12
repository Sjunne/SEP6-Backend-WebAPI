﻿using BuissnessLogic.Handlers;
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

        public MovieController(IDaFactory daFactory)
        {
            _movieHandler = new MovieHandler(daFactory.MoviesRepository());
            _obmHandler = new OmdbHandler(new System.Net.Http.HttpClient());
        }

        [HttpGet]
        [Route("title/{title}")]
        public MovieDa GetByTitle([FromRoute] string title)
        {
            return _movieHandler.GetMovieByTitle(title);
        }

        [HttpGet]
        [Route("/{id}")]
        public MovieDa GetgetFullMovie([FromRoute] string id)
        {
            return null;
            //return _obmHandler.GetFullMovie(id);
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
    }
}