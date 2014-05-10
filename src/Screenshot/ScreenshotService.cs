using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using ServiceStack;

namespace Screenshot
{
    public class ScreenshotService : Service
    {
        public object Get(GetScreenshot request)
        {
            var template = ReadJsTemplate();

            var result = request.SavePath.MapHostAbsolutePath();
            var tokenValues = new Dictionary<string, string>
            {
                {"#URL#", request.Url},
                {"#SAVE_PATH#", result}
            };

            var outputText = PopulateTemplate(template, tokenValues);
            var tmp = "~/../../screenshots/tmp.js".MapHostAbsolutePath();
            File.WriteAllText(tmp, outputText);
            var phantomJsPath = "~/../../ext/phantomjs/phantomjs.exe".MapHostAbsolutePath();

            var psi = new ProcessStartInfo(phantomJsPath);
            psi.Arguments = "'"+ tmp + "'";
            psi.UseShellExecute = true;
            psi.WorkingDirectory = "~/../../".MapHostAbsolutePath();
            psi.WindowStyle = ProcessWindowStyle.Normal;

            Process.Start(psi);

            return new
            {
                PhantomJsPath = phantomJsPath,
                TmpFile = tmp,
                ResultPath = result ,
                WorkingDir = "~/../../".MapHostAbsolutePath()
            };
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
            var path = "~/../../screenshots/template.js".MapHostAbsolutePath();
            return File.ReadAllText(path);
        }
    }



    [Route("/screenshot/", "GET")]
    public class GetScreenshot
    {
        public string Url { get; set; }
        public string SavePath { get; set; }
    }
}