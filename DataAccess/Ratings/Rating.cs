using Dapper.Contrib.Extensions;

namespace DataAccess.Ratings
{
//    [Table("people")]
    public class Rating
    {
        public string gmail { get; set; }
        public string movieid { get; set; }
        public int rating { get; set; }
    }
}