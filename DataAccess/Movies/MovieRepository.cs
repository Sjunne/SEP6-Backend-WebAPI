using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Dapper;
using System.Linq;
using Microsoft.VisualBasic;
using DataAccess.Comments;

namespace DataAccess.Movies
{
    public class MovieRepository : IMovieRepository
    {
        private readonly IDbConnection _connection;

        public MovieRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public MovieDa GetMovieByTitle(string title)
        {
            const string query = @"SELECT * FROM movies WHERE title = @title";
            return _connection.Query<MovieDa>(query, new {title}).FirstOrDefault();
        }

        public List<MovieDa> GetMoviesAndImagesByTitle(string title)
        {
            const string query = @"SELECT TOP(5) * FROM movies WHERE title LIKE @title + '%' 
                                    ORDER BY CASE WHEN title = @title THEN 1 ELSE 2 END";
                return (List<MovieDa>)_connection.Query<MovieDa>(query, new { title });
        }

        public FavoriteMovieModel SetFavorite(FavoriteMovieModel model)
        {
            var username = model.username;
            var movieId = model.movieId;
            var myBool = model.favorite;
            const string query = @"IF EXISTS(select * from favorites where username = @username AND movieId = @movieId)
                                 update favorites set favorite = @myBool where username = @username AND movieId = @movieId
                                 ELSE
                                 insert into favorites(username, movieId, favorite) values(@username, @movieId, @myBool);";
           
            return (FavoriteMovieModel)_connection.QueryFirstOrDefault<FavoriteMovieModel>(query, new { username, movieId, myBool });
        }

        FavoriteMovieModel IMovieRepository.CheckFavorite(string email, string id)
        {
            const string query = @"SELECT * FROM favorites WHERE username = @email AND movieId = @id";
            return (FavoriteMovieModel)_connection.QueryFirstOrDefault<FavoriteMovieModel>(query, new { email, id });
        }

        List<FavoriteMovieModel> IMovieRepository.GetAllFavorites(string userProfile)
        {
            const string query = @"SELECT * FROM favorites WHERE username = @userProfile";
            return (List<FavoriteMovieModel>)_connection.Query<FavoriteMovieModel>(query, new { userProfile });
        }
    }
}
