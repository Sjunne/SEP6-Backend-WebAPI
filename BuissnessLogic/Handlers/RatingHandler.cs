using DataAccess.Ratings;

namespace BuissnessLogic.Handlers
{
    public class RatingHandler
    {
        
        private IRatingRespository _respository;

        public RatingHandler(IRatingRespository respository)
        {
            _respository = respository;
        }


        public bool Rate(Rating rating)
        {
            return _respository.Rate(rating);
        }
    }
}