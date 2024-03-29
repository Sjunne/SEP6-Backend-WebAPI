﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BuissnessLogic.Handlers;
using DataAccess.Actors;
using System;
using DataAccess.Factories;


namespace SEP6_Backend_WebAPI.Controllers
{
    [Route("api/v1/actor")]
    [ApiController]
    public class ActorController : Controller
    {
        private readonly ActorHandler _actorHandler;
        private readonly TmdbHandler _tmdbHandler;
        public ActorController(IDaFactory daFactory)
        {
            _actorHandler = new ActorHandler(daFactory.ActorRepository());
            _tmdbHandler = new TmdbHandler(new System.Net.Http.HttpClient());
        }
        
        [HttpGet]
        [Route("nameandbirth/{keyword}")]
        public List<Actor> GetAllByKeyword([FromRoute] string keyword)
        {
            var actorList = _actorHandler.GetActorsByKeyword(keyword);
            return actorList;
        }
        
        [HttpGet]
        [Route("search/{keyword}")]
        public List<FullPerson> GetAllBySearch([FromRoute] string keyword)
        {
            var fullPersonList = _tmdbHandler.SearchPersonByName(keyword).Result;
            return fullPersonList;
        }

        [HttpGet]
        [Route("searchpersonbyid/{id}")]
        public PersonDetail GetPersonById([FromRoute] string id)
        {
            var p = _tmdbHandler.SearchPersonById(id).Result;
            return p;
        }
        
        [HttpGet]
        [Route("fullcredits/{id}")]
        public List<Cast> FullCredits([FromRoute] string id)
        {
            var p = _tmdbHandler.GetFullCreditAsCast(id);
            return p;
        }
        
        [HttpGet]
        [Route("fullcredits/crew/{id}")]
        public List<Crew> FullCreditsForCrew([FromRoute] string id)
        {
            var p = _tmdbHandler.GetFullCreditAsCrew(id);
            return p;
        }
        
        [HttpGet]
        [Route("people/popular")]
        public List<FullPerson> GetPopularActors()
        {
            var p = _tmdbHandler.GetPopularActors().Result;
            return p;
        }
    }
}