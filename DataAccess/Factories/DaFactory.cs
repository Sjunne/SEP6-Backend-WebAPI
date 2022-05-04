using DataAccess.Movies;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DataAccess.Factories
{
    public class DaFactory : IDaFactory
    {
        private readonly IDbConnection _connection;

        public DaFactory(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public IMovieRepository MoviesRepository()
        {
            return new MovieRepository(_connection);
        }
    }

}
