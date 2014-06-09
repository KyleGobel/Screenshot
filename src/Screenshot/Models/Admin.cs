using ServiceStack.DataAnnotations;

namespace Screenshot.Models
{
    [Schema("dbo")]
    public class Admin
    {
        [Alias("admin_id"), PrimaryKey]
        public string Username { get; set; }

        [Alias("admin_pass")]
        public string Password { get; set; }

        [Alias("admin_level")]
        public int? Level { get; set; }

        [Alias("display_name")]
        public string DisplayName { get; set; }
         
    }
}