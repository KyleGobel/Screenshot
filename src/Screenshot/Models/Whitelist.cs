using System;
using ServiceStack.DataAnnotations;

namespace Screenshot.Models
{
    [Schema("dbo"),Alias("Link_Checker_Whitelist")]
    public class Whitelist
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Alias("Date_Added")]
        public DateTime DateAdded { get; set; }

        public string Url { get; set; }
    }
}