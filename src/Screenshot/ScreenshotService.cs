using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using Amazon.S3;
using Amazon.S3.Model;
using Screenshot.Models;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Logging;

namespace Screenshot
{
    public class ScreenshotService : Service
    {
        public ILog Logger { get; set; }
        public object Get(GetScreenshot request)
        {
            Logger.Debug("Getting a screenshot");
            var screenShotFilePath = GetScreenShot(request.Url);
            Logger.DebugFormat("got screenshot and placed at filepath {0}", screenShotFilePath);
            var urlId = GetOrCreateLinkUrlId(request.Url);

            var resultFilePath = RenameFileToUrlId(screenShotFilePath, urlId);
            UploadScreenShotToS3(resultFilePath, AppHost.AwsAccessKey, AppHost.AwsSecretKey);


            //delete the file after uploading to s3
            if (File.Exists(resultFilePath))
                File.Delete(resultFilePath);

            var imageUrl = SaveUrlImage(Path.GetFileName(resultFilePath), urlId);
            return new
            {
                UrlId = urlId,
                ImageUrl = imageUrl
            };
        }

        private string SaveUrlImage(string getFileName, long urlId)
        {
            var imageUrl = "http://s3.amazonaws.com/BadLinkScreenShots/" + getFileName;
            var imageService = ResolveService<LinkImageService>();

            imageService.Post(new AddImage
            {
                ImageUrl = imageUrl,
                UrlId = urlId
            });
            return imageUrl;
        }

        string RenameFileToUrlId(string imageFilePath, long urlId)
        {
            var newFilePath = Path.Combine(Path.GetDirectoryName(imageFilePath), urlId + ".png");

            if (File.Exists(newFilePath))
                File.Delete(newFilePath);

            File.Copy(imageFilePath,newFilePath);
            return newFilePath;
        }

        long GetOrCreateLinkUrlId(string url)
        {
            var linkUrlService = ResolveService<LinkUrlService>();
            var urlId = default(long);

            //get or create linkUrl
            var linkUrl = linkUrlService.Get(new FindLinkUrl {ExactUrl = url});
            if (linkUrl.IsErrorResponse())
            {
                var addResult = linkUrlService.Post(new AddLinkUrl {Url = url})
                    .GetResponseDto<LinkUrls>();

                urlId = addResult.Id;
            }
            else
            {
                urlId = linkUrl.GetResponseDto<LinkUrls>().Id;
            }

            return urlId;
        }

        string GetScreenShot(string url)
        {
            Logger.Debug("Attempting to read template");
            var template = ReadJsTemplate();
            Logger.Debug("found and read the js template");
            var savePath = "screenshot/" + Guid.NewGuid().ToString("N") + ".png";
            var tokenValues = new Dictionary<string, string>
            {
                {"#URL#", url},
                {"#SAVE_PATH#", savePath}
            };

            var outputText = PopulateTemplate(template, tokenValues);
            var tmp = "~/screenshot/tmp.js".MapHostAbsolutePath();
            File.WriteAllText(tmp, outputText);
            var phantomJsPath = "~/phantomjs/phantomjs.exe".MapHostAbsolutePath();

            var psi = new ProcessStartInfo(phantomJsPath)
            {
                Arguments = "'" + tmp + "'",
                UseShellExecute = true,
                WorkingDirectory = "~/".MapHostAbsolutePath(),
                WindowStyle = ProcessWindowStyle.Normal
            };

            var process = Process.Start(psi);

            //wait for one min max
            if (process != null)
            {
                process.WaitForExit(1000*60); 
            }

            return  ("~/" + savePath).MapHostAbsolutePath();
        }
        static string UploadScreenShotToS3(string filename, string awsAccessKey, string awsSecretKey)
        {
            using (var client = Amazon.AWSClientFactory.CreateAmazonS3Client(awsAccessKey, awsSecretKey, new AmazonS3Config { ServiceURL = "http://s3.amazonaws.com" }))
            {
                var request = new PutObjectRequest
                {
                    BucketName = "BadLinkScreenShots",
                    FilePath = filename,
                    CannedACL = S3CannedACL.PublicRead,
                    Key = Path.GetFileName(filename)
                };
                client.PutObject(request);
            }
            return Path.GetFileName(filename);
        }
        private string PopulateTemplate(string template, Dictionary<string, string> tokenValues)
        {
            foreach (var kvp in tokenValues)
            {
                template = template.ReplaceAll(kvp.Key,"\"" + kvp.Value.Replace('"', '\'') + "\"");
            }
            return template;
        }
        private string ReadJsTemplate()
        {
            var path = "~/screenshot/template.js".MapHostAbsolutePath();
            Logger.DebugFormat("Reading template from {0}",path);
            return File.ReadAllText(path);
        }
    }



    [Route("/screenshot/", "GET")]
    public class GetScreenshot
    {
        public string Url { get; set; }
    }
}