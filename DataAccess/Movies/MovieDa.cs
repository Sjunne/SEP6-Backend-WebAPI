using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Movies
{
    [Table("movies")]
    public class MovieDa
    {
        [ExplicitKey]
        public string Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string PosterHttp { get; set; }
        public string Plot { get; set; }
    }
}
