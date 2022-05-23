using BuissnessLogic.Handlers;
using DataAccess.Comments;
using DataAccess.Factories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace SEP6_Backend_WebAPI.Controllers
{
    [Route("api/v1/comments")]
    [ApiController]
    public class CommentController : Controller
    {
        private readonly CommentHandler _commentHandler;


        public CommentController(IDaFactory daFactory)
        {
            _commentHandler = new CommentHandler(daFactory.CommentRepository());
            
        }

       [HttpPost]
       [Route("CreateComment")]
       public Comment CreateComment(Comment comment)
        {
            return _commentHandler.CreateComment(comment);
        }

        [HttpGet]
        [Route("GetAll/{id}")]
        public List<Comment> GetComments([FromRoute] string id)
        {
            return _commentHandler.GetComments(id);
        }

        [HttpPost]
        [Route("updateComment")]
        public Comment UpdateComment(Comment comment)
        {
            return _commentHandler.UpdateComment(comment);
        }

        [HttpDelete]
        [Route("deleteComment/{id}")]
        public void DeleteComment(string id)
        {
            _commentHandler.DeleteComment(id);
        }

    }
}
