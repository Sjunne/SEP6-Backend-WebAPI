using DataAccess.Actors;
using System.Collections.Generic;

namespace BuissnessLogic.Handlers
{
    public class ActorHandler
    {
        private IActorRepository _actorRepository;
        
        public ActorHandler(IActorRepository actorRepository)
        {
            _actorRepository = actorRepository;
        }
        
        public List<Actor> GetActorsByKeyword(string keyword)
        {
            return _actorRepository.GetActorsByKeyword(keyword);
        }
    }
}