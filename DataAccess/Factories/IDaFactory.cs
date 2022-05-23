using DataAccess.Movies;
using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Actors;
using DataAccess.Ratings;

namespace DataAccess.Factories
{
    public interface IDaFactory
    {
        IMovieRepository MoviesRepository();
        IActorRepository ActorRepository();

        IRatingRespository RateRepository();
    }
}
