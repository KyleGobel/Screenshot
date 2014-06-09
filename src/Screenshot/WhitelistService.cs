using System;
using System.Net;
using Amazon.EC2;
using Screenshot.Models;
using ServiceStack;
using ServiceStack.OrmLite;

namespace Screenshot
{
    public class WhitelistService : Service
    {
        public object Get(GetWhitelistSince request)
        {
            if (request.SinceDate != default(DateTime))
                return Db.Select<Whitelist>(x => x.DateAdded > request.SinceDate);

            var date = DateTime.Now.Subtract(TimeSpan.FromDays(30));
            return Db.Select<Whitelist>(x => x.DateAdded > date);
        }

        public object Post(AddEntryToWhitelist request)
        {
            var date = DateTime.Now;

            Db.Insert(new Whitelist {DateAdded = date, Url = request.Url});
            return new HttpResult()
            {
                StatusCode = HttpStatusCode.Created
            };
        }

        public object Delete(DeleteEntryFromWhitelist request)
        {
            Db.Delete<Whitelist>(x => x.Url == request.Url);
            return new HttpResult
            {
                StatusCode = HttpStatusCode.NoContent
            };
        }
    }

    [Route("/whitelist", "DELETE")]
    public class DeleteEntryFromWhitelist
    {
        public string Url { get; set; }
    }

    [Route("/whitelist", "POST")]
    public class AddEntryToWhitelist
    {
        public string Url { get; set; }
    }

    [Route("/whitelist", "GET")]
    public class GetWhitelistSince
    {
        public DateTime SinceDate { get; set; }
    }
}