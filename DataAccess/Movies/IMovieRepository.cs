using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Movies
{
    public interface IMovieRepository
    {
        MovieDa GetMovieByTitle(string title);
    }
}
