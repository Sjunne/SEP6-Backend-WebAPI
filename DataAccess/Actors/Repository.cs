using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Linq;

namespace DataAccess.Actors
{
    public class Repository : IActorRepository
    {
        private readonly IDbConnection _connection;

        public Repository(IDbConnection connection)
        {
            _connection = connection;
        }

        public List<Actor> GetActorsByKeyword(string keyword)
        {
            const string query = @"SELECT TOP(5) * FROM peoplev2 WHERE name LIKE @keyword + '%'";
            return (List<Actor>)_connection.Query<Actor>(query, new { keyword });
        }
    }
}