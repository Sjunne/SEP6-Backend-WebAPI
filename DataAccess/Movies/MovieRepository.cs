using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Dapper;
using System.Linq;
using Microsoft.VisualBasic;
using DataAccess.Comments;
using Dapper.Contrib.Extensions;
using System.Data.SqlClient;

namespace DataAccess.Movies
{
    public class MovieRepository : IMovieRepository
    {
        private readonly string _connection;

        public MovieRepository(string connection)
        {
            _connection = connection;
        }

        public MovieDa GetMovieByTitle(string title)
        {
            using (var _connection2 = new SqlConnection(_connection))
            {
                const string query = @"SELECT * FROM movies WHERE title = @title";
                return _connection2.Query<MovieDa>(query, new { title }).FirstOrDefault();
            }
        }

        public List<MovieDa> GetMoviesAndImagesByTitle(string title)
        {
            using (var _connection2 = new SqlConnection(_connection))
            {
                const string query = @"SELECT TOP(5) * FROM movies WHERE title LIKE @title + '%' 
                                    ORDER BY CASE WHEN title = @title THEN 1 ELSE 2 END";
                return (List<MovieDa>)_connection2.Query<MovieDa>(query, new { title });
            }
        }

        public FavoriteMovieModel SetFavorite(FavoriteMovieModel model)
        {
            using (var _connection2 = new SqlConnection(_connection))
            {
                var username = model.username;
                var movieId = model.movieId;
                var myBool = model.favorite;
                const string query = @"IF EXISTS(select * from favorites where username = @username AND movieId = @movieId)
                                 update favorites set favorite = @myBool where username = @username AND movieId = @movieId
                                 ELSE
                                 insert into favorites(username, movieId, favorite) values(@username, @movieId, @myBool);";

                return (FavoriteMovieModel)_connection2.QueryFirstOrDefault<FavoriteMovieModel>(query, new { username, movieId, myBool });
            }
        }

        FavoriteMovieModel IMovieRepository.CheckFavorite(string email, string id)
        {
            using (var _connection2 = new SqlConnection(_connection))
            {
                const string query = @"SELECT * FROM favorites WHERE username = @email AND movieId = @id";
                return (FavoriteMovieModel)_connection2.QueryFirstOrDefault<FavoriteMovieModel>(query, new { email, id });
            }
        }

        List<FavoriteMovieModel> IMovieRepository.GetAllFavorites(string userProfile)
        {
            using (var _connection2 = new SqlConnection(_connection))
            {
                const string query = @"SELECT [username],[movieId], [favorite], [title] FROM favorites 
                                   INNER JOIN dbo.movies ON movieId = movies.id WHERE username = @userProfile";
                return (List<FavoriteMovieModel>)_connection2.Query<FavoriteMovieModel>(query, new { userProfile });
            }
        }
    }
}