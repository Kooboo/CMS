using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.CMS.Sites.Web
{
    public class KoobooCMSResponseModule : IHttpModule
    {
        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += new EventHandler(context_PreRequestHandlerExecute);
        }

        void context_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("X-KoobooCMS-Version", Kooboo.CMS.Sites.KoobooCMSVersion.Version);
        }
    }
}
