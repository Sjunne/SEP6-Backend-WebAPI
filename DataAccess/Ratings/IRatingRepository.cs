using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Ratings
{
    public interface IRatingRespository
    {
        void Rate(Rating rating);
        int GetRating(string info);
        double AverageRating(string movieid);
        void DeleteRating(string id);
    }
}