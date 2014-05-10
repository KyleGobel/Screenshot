using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Funq;
using ServiceStack;

namespace Screenshot
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base("Screenshot", typeof (ScreenshotService).Assembly) {}

        public override void Configure(Container container)
        {

        }
    }
}