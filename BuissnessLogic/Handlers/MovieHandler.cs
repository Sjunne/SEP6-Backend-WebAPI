using DataAccess.Movies;
using System;
using System.Collections.Generic;
using System.Reflection;
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

        public FavoriteMovieModel SetFavorite(FavoriteMovieModel model)
        {
            return _movieRepository.SetFavorite(model);
        }

        public FavoriteMovieModel CheckFavorite(string email, string id)
{
            return _movieRepository.CheckFavorite(email, id);
        }

        public List<FavoriteMovieModel> GetAllFavorites(string userProfile)
        {
            return _movieRepository.GetAllFavorites(userProfile);
        }
    }
}
