using ServiceStack.DataAnnotations;

namespace Screenshot.Models
{
    public class LinkUrls
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public string Url { get; set; }
    }
}