using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Comments
{
    public interface ICommentRepository
    {
        Comment CreateComment(Comment comment);
        List<Comment> GetComments(string id);
        Comment UpdateComment(Comment comment);
        void DeleteComment(string id);
    }
}
