using Funq;
using Screenshot.Models;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Text;

namespace Screenshot
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base("Screenshot", typeof (ScreenshotService).Assembly) {}

        public static string AwsAccessKey;
        public static string AwsSecretKey;
        public override void Configure(Container container)
        {
            JsConfig.EmitCamelCaseNames = true;
            var appSettings = new TextFileSettings("~/appSettings.txt".MapHostAbsolutePath(), ":");

            container.Register<IDbConnectionFactory>(c => new OrmLiteConnectionFactory(
                appSettings.Get("database"), 
                SqlServerDialect.Provider));

            AwsAccessKey = appSettings.Get("awsAccessKey");
            AwsSecretKey = appSettings.Get("awsSecretKey");


            using (var db = container.Resolve<IDbConnectionFactory>().Open())
            {
                db.CreateTableIfNotExists<LinkUrls>();
                db.CreateTableIfNotExists<LinkImages>();
            }

            Routes.Add<FindLinkUrl>("/urls/{Id}", "GET")
                .Add<AddLinkUrl>("/urls", "POST")
                .Add<StartGetImages>("/start", "GET");

        }
    }
}