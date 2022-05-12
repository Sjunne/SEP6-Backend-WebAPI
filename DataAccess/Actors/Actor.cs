using Dapper.Contrib.Extensions;

namespace DataAccess.Actors
{
    [Table("people")]
    public class Actor
    {
        [ExplicitKey]
        public string Id { get; set; }
        public string Name { get; set; }
        public int Birth { get; set; }
    }
}