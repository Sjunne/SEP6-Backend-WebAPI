using Dapper.Contrib.Extensions;

namespace DataAccess.Ratings
{
//    [Table("people")]
    public class Rating
    {
        public string gmail { get; set; }
        [ExplicitKey]
        public string id { get; set; }
        public int rating { get; set; }
    }
}