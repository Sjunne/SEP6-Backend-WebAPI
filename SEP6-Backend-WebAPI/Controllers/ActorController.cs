using Microsoft.AspNetCore.Mvc;
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

        public ActorController(IDaFactory daFactory)
        {
            _actorHandler = new ActorHandler(daFactory.ActorRepository());
        }
        
        [HttpGet]
        [Route("nameandbirth/{keyword}")]
        public List<Actor> GetAllByKeyword([FromRoute] string keyword)
        {
            var actorList = _actorHandler.GetActorsByKeyword(keyword);
            return actorList;
        }
    }
}