using System.Data.Common;
using System.Net;
using Screenshot.Models;
using ServiceStack;
using ServiceStack.OrmLite;

namespace Screenshot
{
    public class LinkUrlService : Service
    {
        public object Get(FindLinkUrl request)
        {
            var result = default(LinkUrls);
            if (request.Id != default(long))
                result =  Db.SingleById<LinkUrls>(request.Id);

            if (request.ExactUrl != default(string))
                result =  Db.SingleWhere<LinkUrls>("Url", request.ExactUrl);

            if (result == null)
                return HttpError.NotFound("No Link Urls found");

            return result;
        }

        public object Post(AddLinkUrl request)
        {
            var identity = Db.Insert(new LinkUrls {Url = request.Url}, true);

            var newObj = new LinkUrls {Id = identity, Url = request.Url};

            return new HttpResult(newObj, HttpStatusCode.Created)
            {
                Location = new FindLinkUrl {Id = identity}.ToGetUrl()
            };
        }
         
    }

    public class AddLinkUrl
    {
        public string Url { get; set; }
    }

    public class FindLinkUrl
    {
        public long Id { get; set; }
        public string ExactUrl { get; set; }
    }
}