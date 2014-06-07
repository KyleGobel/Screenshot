using System.Linq;
using System.Net;
using Amazon.Glacier;
using Screenshot.Models;
using ServiceStack;
using ServiceStack.OrmLite;

namespace Screenshot
{
    public class LinkImageService : Service
    {
        public object Get(FindImage request)
        {
            if (request.Url != default(string))
            {
                return Db.Single<LinkImages>(x => x.ImageUrl == request.Url);
            }

            if (request.UrlId != default(int))
            {
                return Db.Single<LinkImages>(x => x.UrlId == request.UrlId);
            }

            return HttpError.NotFound("Couldn't find link image");
        }

        public object Get(StartGetImages request)
        {

            var ssService = ResolveService<ScreenshotService>();
            
           var linksWithoutImages = Db.SqlList<BadLinksWithoutImages>("exec usp_get_links_without_images")
               .Select(x => x.Url)
               .ToList();

            linksWithoutImages.ForEach(x => ssService.Get(new GetScreenshot {Url = x}));

            return "finished";
        }

        public object Put(ModifyImage request)
        {
            Db.UpdateOnly(new LinkImages {ImageUrl = request.ImageUrl}, x => x.ImageUrl, x => x.UrlId == request.UrlId);

            return new HttpResult(HttpStatusCode.NoContent, "Updated");
        }
        public object Post(AddImage request)
        {
            var alreadyExists = Db.Exists<LinkImages>(x => x.UrlId == request.UrlId);

            if (alreadyExists)
            {
                return Put(new ModifyImage {ImageUrl = request.ImageUrl, UrlId = request.UrlId});
            }
            var newImage = new LinkImages
            {
                ImageUrl = request.ImageUrl,
                UrlId = request.UrlId
            };

            var newImageId = Db.Insert(newImage, true);
            newImage.Id = newImageId;

            return new HttpResult(newImage, HttpStatusCode.Created);
        }
         
    }

    public class StartGetImages
    {
    }

    public class ModifyImage
    {
        public long UrlId { get; set; }
        public string ImageUrl { get; set; }
    }

    public class AddImage
    {
        public string ImageUrl { get; set; }
        public long UrlId { get; set; }
    }

    public class FindImage
    {
        public string Url { get; set; }
        public int UrlId { get; set; }
        public int LinkId { get; set; }
        public int BadLinkId { get; set; }
    }
}