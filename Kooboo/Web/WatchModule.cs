using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.Web
{
    public class WatchModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += OnBeginRequest;
            context.EndRequest += OnEndRequest;
        }

        private void OnBeginRequest(object sender, System.EventArgs e)
        {
            if (HttpContext.Current.Request.IsLocal
                && HttpContext.Current.IsDebuggingEnabled)
            {
                string path = HttpContext.Current.Request.Url.AbsolutePath;
                if (path.Length > 4 && (path[path.Length - 3] == '.' || path[path.Length - 4] == '.'))
                    return;

                var stopwatch = new Stopwatch();
                HttpContext.Current.Items["Stopwatch"] = stopwatch;
                stopwatch.Start();
            }
        }

        private void OnEndRequest(object sender, System.EventArgs e)
        {
            if (HttpContext.Current.Items.Contains("Stopwatch") && HttpContext.Current.Response.ContentType == "text/html")
            {
                Stopwatch stopwatch =
                  (Stopwatch)HttpContext.Current.Items["Stopwatch"];
                stopwatch.Stop();

                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0}ms", ts.TotalMilliseconds);

                HttpContext.Current.Response.Write("<p>" + elapsedTime + "</p>");
            }
        }
    }
}
