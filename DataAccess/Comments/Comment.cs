using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Comments
{
    public class Comment
    {
        [ExplicitKey]
        public string id { get; set; }
        public string body { get; set; }
        public string username { get; set; }
        public string userId { get; set; }
        public string createdAt { get; set; }
        public string parentId { get; set; }
        public string movieId { get; set; }
    }
}
