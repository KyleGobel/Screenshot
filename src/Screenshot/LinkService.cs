using Screenshot.Models;
using ServiceStack;
using ServiceStack.OrmLite;

namespace Screenshot
{
    public class LinkService : Service
    {
        public object Get(FindLinks request)
        {
            var jn = new JoinSqlBuilder<LinkEx, Link>();
            jn = jn.Join<Link, Page>(x => x.PageId, x => x.Id, x => new {x.Id, x.Title, x.Url},
                x => new {PageTitle = x.Title});

            if (request.Id != default(int))
            {
                jn.Where<Link>(x => x.Id == request.Id);

                var sql = jn.ToSql();
                return Db.Select<LinkEx>(sql);
            }

            if (request.Title != default(string) && request.Url != default(string))
            {
                if (request.ExactMatch)
                    jn.Where<Link>(x => x.Title == request.Title && x.Url == request.Url);
                else
                {
                    jn.Where<Link>(x => x.Title.Contains(request.Title) && x.Url.Contains(request.Url));
                }

                var sql = jn.ToSql();
                return Db.Select<LinkEx>(sql);
            }

            if (request.Title != default(string))
            {
                if (request.ExactMatch)
                    jn.Where<Link>(x => x.Title == request.Title);
                else
                {
                    jn.Where<Link>(x => x.Title.Contains(request.Title) );
                }

                var sql = jn.ToSql();
                return Db.Select<LinkEx>(sql);
            }

            if (request.Url != default(string))
            {
                if (request.ExactMatch)
                    jn.Where<Link>(x => x.Url == request.Url);
                else
                {
                    jn.Where<Link>(x => x.Url.Contains(request.Url));
                }

                var sql = jn.ToSql();
                return Db.Select<LinkEx>(sql);
            }

            return "nothing";
        }
         
    }

    [Route("/links", "GET")]
    public class FindLinks
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public bool ExactMatch { get; set; }
    }
}