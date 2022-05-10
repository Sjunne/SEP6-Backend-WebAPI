using DataAccess.Movies;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuissnessLogic.Handlers
{
    public class MovieHandler
    {
        private IMovieRepository _movieRepository;

        public MovieHandler(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public MovieDa GetMovieByTitle(string title)
        {
            return _movieRepository.GetMovieByTitle(title);
        }
        
        public List<MovieDa> GetMoviesByTitle(string title)
        {
            return _movieRepository.GetMoviesAndImagesByTitle(title);
        }
    }
}
