using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Movies
{
    public interface IMovieRepository
    {
        MovieDa GetMovieByTitle(string title);
        List<MovieDa> GetMoviesAndImagesByTitle(string title);
        FavoriteMovieModel SetFavorite(FavoriteMovieModel model);
        FavoriteMovieModel CheckFavorite(string email, string id);
        List<FavoriteMovieModel> GetAllFavorites(string userProfile);
    }
}
