using DataAccess.Movies;
using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Actors;
using DataAccess.Comments;

namespace DataAccess.Factories
{
    public interface IDaFactory
    {
        IMovieRepository MoviesRepository();
        IActorRepository ActorRepository();
        ICommentRepository CommentRepository();
    }
}
