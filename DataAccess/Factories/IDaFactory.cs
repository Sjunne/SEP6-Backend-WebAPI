using DataAccess.Movies;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Factories
{
    public interface IDaFactory
    {
        IMovieRepository MoviesRepository();
    }
}
