using ServiceStack.DataAnnotations;

namespace Screenshot.Models
{
    public class LinkImages
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; } 

        [ForeignKey(typeof(LinkUrls))]
        public long UrlId { get; set; }
        public string ImageUrl { get; set; }
    }
}