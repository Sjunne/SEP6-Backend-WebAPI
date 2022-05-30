using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;

namespace DataAccess.Ratings
{
    public class RatingRepository : IRatingRespository
    {
        private readonly string _connection;

        public RatingRepository(string connection)
        {
            _connection = connection;
        }
        
        private void DeleteRating(string id, string gmail)
        {
            using(var _connection2 =  new SqlConnection(_connection))
            {
                _connection2.Execute(@"DELETE FROM sepdb.dbo.rating WHERE sepdb.dbo.rating.movieid = @id AND sepdb.dbo.rating.gmail = @gmail", new { id = id, gmail = gmail });
            }
        }

        private Rating CreateRating(Rating rating)
        {
            using (var _connection2 = new SqlConnection(_connection))
            {
                var gmail = rating.gmail;
                var movieid = rating.movieid;
                var rate = rating.rating;
                const string query = @"INSERT INTO rating (gmail, movieid, rating) 
            OUTPUT INSERTED.*
            VALUES (@gmail, @movieid, @rate)";
                var output = _connection2.QuerySingle<Rating>(query, new { gmail, movieid, rate });
                return output;
            }
        }

        private List<Rating> GetRatings(string id, string gmail)
        {
            using (var _connection2 = new SqlConnection(_connection))
            {
                const string query = @"SELECT * FROM rating WHERE movieid = @id AND gmail = @gmail";
                return (List<Rating>)_connection2.Query<Rating>(query, new { id, gmail });
            }
        }
        
        private bool Exists(string id)
        {
            using (var _connection2 = new SqlConnection(_connection))
            {
                const string query = @"SELECT * FROM sepdb.dbo.rating WHERE sepdb.dbo.rating.movieid = @id";
                var l = (List<Rating>)_connection2.Query<Rating>(query, new { id });
                return l.Count > 0;
            }
        }

        private Rating UpdateComment(Rating rating)
        {
            using (var _connection2 = new SqlConnection(_connection))
            {
                var gmail = rating.gmail;
                var movieid = rating.movieid;
                var rate = rating.rating;
                const string query = @"UPDATE rating SET rating = @rate WHERE movieid = @movieid AND gmail = @gmail";

                _connection2.Query<Rating>(query, new { rate, movieid, gmail });
                return _connection2.Get<Rating>(movieid);
            }
        }

        public void Rate(Rating rating)
        {
            List<Rating> l = GetRatings(rating.movieid, rating.gmail);
            if (l.Count == 0)
            {
                CreateRating(rating);
            }
            else
            {
                UpdateComment(rating);
            }
        }

        public int GetRating(string info)
        {  
            var strs = info.Split(",");

            var gr = GetRatings(strs[0], strs[1]).First();
            if (gr == null)
            {
                return 0;
            }

            return gr.rating;
        }

        public double AverageRating(string movieid)
        {
            using (var _connection2 = new SqlConnection(_connection))
            {
                if (Exists(movieid))
                {
                    const string query = @"SELECT AVG(sepdb.dbo.rating.rating) FROM sepdb.dbo.rating WHERE sepdb.dbo.rating.movieid = @movieid;";
                    return _connection2.Query<double>(query, new { movieid }).First();
                }

                return 0;
            }
        }

        public void DeleteRating(string id)
        {
            var strs = id.Split(",");
            DeleteRating(strs[0], strs[1]);
        }
    }
}