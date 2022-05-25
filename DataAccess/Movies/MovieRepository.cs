using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Dapper;
using System.Linq;
using Dapper.Contrib.Extensions;

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
       
    }
}