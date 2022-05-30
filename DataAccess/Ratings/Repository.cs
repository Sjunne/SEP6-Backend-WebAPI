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
                _connection2.Execute(@"DELETE FROM rating WHERE rating.id = @id AND rating.gmail = @gmail", new { id = id, gmail = gmail });
            }
        }

        private Rating CreateRating(Rating rating)
        {
            using (var _connection2 = new SqlConnection(_connection))
            {
                var gmail = rating.gmail;
                var movieid = rating.id;
                var rate = rating.rating;
                const string query = @"INSERT INTO rating (gmail, id, rating) 
            OUTPUT INSERTED.*
            VALUES (@gmail, @movieid, @rate)";
                var output = _connection2.QuerySingle<Rating>(query, new { gmail, movieid, rate });
                return output;
            }
        }

        private Rating GetRatings(string id, string gmail)
        {
            using (var _connection2 = new SqlConnection(_connection))
            {
                const string query = @"SELECT * FROM rating WHERE id = @id AND gmail = @gmail";
                return (Rating)_connection2.QueryFirstOrDefault<Rating>(query, new { id, gmail });
            }
        }
        
        private bool Exists(string id)
        {
            using (var _connection2 = new SqlConnection(_connection))
            {
                const string query = @"SELECT * FROM rating WHERE rating.id = @id";
                var l = (List<Rating>)_connection2.Query<Rating>(query, new { id });
                return l.Count > 0;
            }
        }

        private Rating UpdateComment(Rating rating)
        {
            using (var _connection2 = new SqlConnection(_connection))
            {
                var gmail = rating.gmail;
                var id = rating.id;
                var rate = rating.rating;
                const string query = @"UPDATE rating SET rating = @rate WHERE rating.id = @id AND gmail = @gmail";

                _connection2.Query<Rating>(query, new { rate, id, gmail });
                return _connection2.Get<Rating>(id);
            }
        }

        public void Rate(Rating rating)
        {
            Rating l = GetRatings(rating.id, rating.gmail);
            if (l == null)
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

            var gr = GetRatings(strs[0], strs[1]);
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
                    const string query = @"SELECT AVG(rating.rating) FROM rating WHERE rating.id = @movieid;";
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