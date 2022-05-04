using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Dapper;
using System.Linq;

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
    }
}
