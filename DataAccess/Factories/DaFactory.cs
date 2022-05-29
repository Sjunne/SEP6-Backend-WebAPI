using DataAccess.Movies;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DataAccess.Actors;
using DataAccess.Comments;

namespace DataAccess.Factories
{
    public class DaFactory : IDaFactory
    {
        private readonly IDbConnection _connection;
        private MovieRepository _movieRepository;
        private Repository _repository;
        private CommentRepository _commentRepository;

        public DaFactory(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public IMovieRepository MoviesRepository()
        {
            if (_movieRepository == null)
                _movieRepository = new MovieRepository(_connection);
            return _movieRepository;
        }
        
        public IActorRepository ActorRepository()
        {
            if(_repository == null)
                _repository = new Repository(_connection);
            return new Repository(_connection);
        }

        public ICommentRepository CommentRepository()
        {
            if (_commentRepository == null)
                _commentRepository = new CommentRepository(_connection);
            return new CommentRepository(_connection);
        }
   
    }

}
