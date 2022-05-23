using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Dapper.Contrib.Extensions;

namespace DataAccess.Ratings
{
    public class RatingRepository : IRatingRespository
    {
        private readonly IDbConnection _connection;

        public RatingRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        
        public bool Rate(Rating rating)
        {
            string gmail = rating.gmail;
            int rate = rating.rating;
            string movieid = rating.movieid;
            const string exists = @"SELECT * FROM dbo.rating 
                                 WHERE dbo.rating.gmail = @gmail AND dbo.rating.movieid = @movieid;";

            var b = (List<Rating>)_connection.Query<Rating>(exists, new {gmail, movieid});

            var r = b.Count == 0;
            if (!r)
            {
                const string insert = @"insert into dbo.rating values ( @gmail, @movieid, @rate);";
                _connection.Insert(insert);
                r = true;
            }
            else
            {
                const string delete = @"drop into dbo.rating values ( @gmail, @movieid, @rate);";
                _connection.Delete();
            }
            return r;
        }
    }
}