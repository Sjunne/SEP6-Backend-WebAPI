using BuissnessLogic.Handlers;
using DataAccess.Factories;
using DataAccess.Movies;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using DataAccess.Ratings;
using Newtonsoft.Json;

namespace SEP6_Backend_WebAPI.Controllers
{
    [Route("api/v1/movie")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly MovieHandler _movieHandler;
        private RatingHandler _handler;

        public RatingController(IDaFactory daFactory)
        {
            _handler = new RatingHandler(daFactory.RateRepository());
            _movieHandler = new MovieHandler(daFactory.MoviesRepository());
        }

        [HttpPost]
        [Route("rating")]
        public bool Rating([FromRoute] Rating rating)
        {
            bool b = _handler.Rate(rating);
            return b;
        }
    }
}