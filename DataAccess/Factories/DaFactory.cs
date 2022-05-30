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
        //private readonly IDbConnection _connection;
        private readonly string _connection;
        private MovieRepository _movieRepository;
        private Repository _repository;
        private CommentRepository _commentRepository;
        private RatingRepository _ratingRepository;

        public DaFactory(string connectionString)
        {
            //_connection = new SqlConnection(connectionString);
            _connection = connectionString;
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
            return _repository;
        }
        
        public ICommentRepository CommentRepository()
        {
            if (_commentRepository == null)
                _commentRepository = new CommentRepository(_connection);
            return _commentRepository;
        }
        public IRatingRespository RateRepository()
        {
            if(_ratingRepository == null)
                _ratingRepository = new RatingRepository(_connection);
            return _ratingRepository;
        }
    }

}
