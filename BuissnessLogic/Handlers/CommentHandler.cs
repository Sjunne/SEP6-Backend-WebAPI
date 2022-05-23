using DataAccess.Comments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace BuissnessLogic.Handlers
{
    public class CommentHandler
    {
        private ICommentRepository _commentRepository;

        public CommentHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public Comment CreateComment(Comment comment)
        {
            return _commentRepository.CreateComment(comment);
        }

        public List<Comment> GetComments(string id)
        {
            return _commentRepository.GetComments(id);
        }

        public Comment UpdateComment(Comment comment)
        {
            return _commentRepository.UpdateComment(comment);
        }

        public void DeleteComment(string id)
{
            _commentRepository.DeleteComment(id);
        }
    }
}
