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
        public void Rating(Rating rating)
        {
             _handler.Rate(rating);
        }
        
        [HttpGet]
        [Route("getrating/{info}")]
        public int GetRating([FromRoute] string info)
        {
            var b = _handler.GetRating(info);
            return b;
        }

        [HttpGet]
        [Route("averagerating/{movieid}")]
        public double AverageRating([FromRoute] string movieid)
        {
            double b = _handler.AverageRating(movieid);
            return b;
        }
        
        [HttpDelete]
        [Route("deleterating/{id}")]
        public void DeleteRating(string id)
        {
            _handler.DeleteRating(id);
        }
    }
}