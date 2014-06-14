using System.Collections;
using ServiceStack.DataAnnotations;

namespace Screenshot.Models
{
    [Schema("dbo"), Alias("Links")]
    public class Link
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Url { get; set; }

        [Alias("Page_ID")]
        public int PageId { get; set; }
    }

    [Schema("dbo"), Alias("Pages")]
    public class Page
    {
        public int Id { get; set; }

        [Alias("Folder_ID")]
        public int FolderId { get; set; }
        public string Title { get; set; }

        [Alias("Auto_Ordering")]
        public bool AutoOrdering { get; set; }

    }

    public class LinkEx
    {
        [BelongTo(typeof(Link))]
        public int Id { get; set; }

        [BelongTo(typeof(Link))]
        public string Title { get; set; }

        [BelongTo(typeof(Link))]
        public string Url { get; set; }

        [BelongTo(typeof(Page)), Alias("Title")]
        public string PageTitle { get; set; }
    }
}