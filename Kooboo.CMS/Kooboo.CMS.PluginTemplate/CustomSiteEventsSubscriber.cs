using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Extension;
using System.Diagnostics;
using System.IO;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.PluginTemplate
{
    public class CustomSiteEventsSubscriber : ISiteEvents
    {
        //static TextWriterTraceListener traceListenser = new TextWriterTraceListener(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Site.log"));
        #region ISiteEventObserver Members

        public void OnSiteStart(Site site)
        {
            //traceListenser.WriteLine(string.Format("{0} Started.", site.FullName));
            //traceListenser.Flush();
        }

        public void OnPreSiteRequestExecute(Site site, System.Web.HttpContextBase httpContext)
        {
            //traceListenser.WriteLine(string.Format("Process {0}.", httpContext.Request.RawUrl));
            //traceListenser.Flush();
        }

        public void OnPostSiteRequestExecute(Site site, System.Web.HttpContextBase httpContext)
        {
            //traceListenser.WriteLine(string.Format("Process finised {0}, response length {1}.", httpContext.Request.RawUrl, httpContext.Response.Filter.Length));
            //traceListenser.Flush();
        }

        public void OnSiteRemoved(Site site)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
