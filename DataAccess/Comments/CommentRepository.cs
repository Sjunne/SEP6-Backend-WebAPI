using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Text;
using Dapper;
using DataAccess.Actors;
using Dapper.Contrib.Extensions;
using DataAccess.Movies;

namespace DataAccess.Comments
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IDbConnection _connection;

        public CommentRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public void DeleteComment(string id)
        {
            _connection.Execute(@"DELETE FROM comments WHERE Id = @Id", new { Id = id });
        }

        Comment ICommentRepository.CreateComment(Comment comment)
        {
            var body = comment.body;
            var username = comment.username;
            var parentId = comment.parentId;
            var createdAt = comment.createdAt;
            var movieId = comment.movieId;
            const string query = @"INSERT INTO comments (body, username, parentId, createdAt, movieId) 
            OUTPUT INSERTED.*
            VALUES (@body, @username, @parentId, @createdAt, @movieId)";
            var output = _connection.QuerySingle<Comment>(query, new { body, username, parentId, createdAt, movieId });
            return output;
        }

        List<Comment> ICommentRepository.GetComments(string id)
        {
            
            const string query = @"SELECT * FROM comments WHERE movieId = @id";
            return (List<Comment>)_connection.Query<Comment>(query, new { id });
        }

        Comment ICommentRepository.UpdateComment(Comment comment)
        {
            var body = comment.body;
            var id = comment.id;
            const string query = @"UPDATE comments SET body = @body WHERE id = @id";
          
            _connection.Query<Comment>(query, new { body, id});
            return _connection.Get<Comment>(id);
        }
    }
}
