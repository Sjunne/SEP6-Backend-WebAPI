using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Linq;
using System.Data.SqlClient;

namespace DataAccess.Actors
{
    public class Repository : IActorRepository
    {
        private readonly string _connection;

        public Repository(string connection)
        {
            _connection = connection;
        }

        public List<Actor> GetActorsByKeyword(string keyword)
        {
            using (var _connection2 = new SqlConnection(_connection))
            {
                const string query = @"SELECT TOP(5) * FROM peoplev2 WHERE name LIKE @keyword + '%'";
                return (List<Actor>)_connection2.Query<Actor>(query, new { keyword });
            }    
        }
    }
}