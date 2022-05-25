using DataAccess.Movies;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DataAccess.Ratings;
using DataAccess.Actors;
using DataAccess.Comments;

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
        
        public IActorRepository ActorRepository()
        {
            return new Repository(_connection);
        }
        
        public ICommentRepository CommentRepository()
        {
            return new CommentRepository(_connection);
        }
        public IRatingRespository RateRepository()
        {
            return new RatingRepository(_connection);
        }
    }

}
