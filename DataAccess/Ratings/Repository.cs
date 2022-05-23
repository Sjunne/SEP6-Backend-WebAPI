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
            if (r)
            {
                const string query = @"INSERT INTO dbo.rating (gmail, movieid, rating)  OUTPUT INSERTED.* VALUES (@gmail, @movieid, @rate)";
                var output = _connection.QuerySingle<Rating>(query, new { gmail, movieid, rate });
                Console.WriteLine(output);
            }
            else
            {
                const string query = @"UPDATE dbo.rating SET rating = @rate WHERE movieid = @movieid AND WHERE gmail = @gmail";
          
                _connection.Query<Rating>(query, new { gmail, movieid, rate});
                var c =  _connection.Get<Rating>(gmail);
                Console.WriteLine(c.gmail);
            }
            return r;
        }
    }
}