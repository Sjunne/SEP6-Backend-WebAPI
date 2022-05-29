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
            _respository.Rate(rating);
            return true;
        }

        public int GetRating(string info)
        {
            return _respository.GetRating(info);
        }

        public double AverageRating(string movieid)
        {
            return _respository.AverageRating(movieid);
        }

        public void DeleteRating(string id)
        {
             _respository.DeleteRating(id);
        }
    }
}