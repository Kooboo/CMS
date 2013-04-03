using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Web;

namespace Kooboo.CMS.Content.FileServer.Web.Services
{
    public static class FileUrlHelper
    {
        static string BaseUri = "";
        static FileUrlHelper()
        {
            BaseUri = System.Web.Configuration.WebConfigurationManager.AppSettings["BaseUri"];
        }
        public static string ResolveUrl(string virtualPath)
        {
            var relativeUri = virtualPath.Replace("~/", "");
            string baseUri = BaseUri;
            if (string.IsNullOrEmpty(baseUri))
            {
                baseUri = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri.ToString();
            }
            return new Uri(new Uri(baseUri), relativeUri).ToString();
        }
    }
}