using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Ratings
{
    public interface IRatingRespository
    {
        bool Rate(Rating rating);
    }
}