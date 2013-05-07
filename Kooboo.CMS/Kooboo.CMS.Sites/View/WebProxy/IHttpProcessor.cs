#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Kooboo.CMS.Sites.View.WebProxy
{

    using Kooboo.CMS.Common.Runtime.Dependency;
    using Kooboo.CMS.Sites.Web;
    using Kooboo.IO;
    using System.IO;
    using System.Net;

    public interface IHttpProcessor
    {
        string ProcessRequest(HttpContextBase httpContext, string url, string httpMethod);
    }
    [Dependency(typeof(IHttpProcessor))]
    public class HttpProcessor : IHttpProcessor
    {
        public virtual string ProcessRequest(HttpContextBase httpContext, string url, string httpMethod)
        {
            var httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);

            FillHttpRequest(httpWebRequest, httpContext, url);
            httpWebRequest.Method = httpMethod;
            if (httpMethod.ToUpper() == "POST")
            {
                var requestStream = httpWebRequest.GetRequestStream();
                var data = httpContext.Request.InputStream.ReadData();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
            }


            var webResponse = httpWebRequest.GetResponse();

            return ProcessWebResponse(webResponse, httpContext);

        }
        protected virtual string ProcessWebResponse(WebResponse webResponse, HttpContextBase httpContext)
        {
            string result = "";
            if (webResponse is HttpWebResponse)
            {
                result = ProcessHttpWebResponse((HttpWebResponse)webResponse, httpContext);
            }

            return result;
        }
        protected virtual string ProcessHttpWebResponse(HttpWebResponse httpWebResponse, HttpContextBase httpContext)
        {
            CombineHttpHeaders(httpWebResponse, httpContext);
            using (var responseStream = httpWebResponse.GetResponseStream())
            {
                var contentType = httpWebResponse.Headers["content-type"];
                // if the content-type is text/html, the response text will conbined with the Kooboo CMS page HTML.
                // or else will output the response stream directly.
                if (string.IsNullOrEmpty(contentType) || contentType.ToLower().Contains("text/html"))
                {
                    using (StreamReader loResponseStream = new StreamReader(responseStream))
                    {
                        string html = loResponseStream.ReadToEnd();
                        return html;
                    }
                }
                else
                {
                    if (httpContext.Response.Output is OutputTextWriterWrapper)
                    {
                        httpContext.Response.Output = ((OutputTextWriterWrapper)httpContext.Response.Output).GetRawOuputWriter();
                    }
                    httpContext.Response.ContentType = contentType;
                    httpContext.Response.Clear();
                    int bytesRead;
                    var buffer = new byte[1024];
                    while ((bytesRead = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        httpContext.Response.OutputStream.Write(buffer, 0, bytesRead);
                    }
                    httpContext.Response.End();

                    return null;
                }
            }
        }

        protected virtual void CombineHttpHeaders(HttpWebResponse httpWebResponse, HttpContextBase httpContext)
        {
            foreach (var key in httpWebResponse.Headers.AllKeys)
            {
                if (ModifiableHeader(key))
                {
                    httpContext.Response.AddHeader(key, httpWebResponse.Headers[key]);
                }
            }
        }
        protected virtual bool ModifiableHeader(string headerName)
        {
            switch (headerName.ToUpper())
            {
                case "CONNECTION":
                case "ACCEPT":
                case "HOST":
                case "REFERER":
                case "USER-AGENT":
                case "CONTENT-LENGTH":
                case "CONTENT-TYPE":
                    return false;
                default:
                    return true;
            }
        }
        protected virtual void FillHttpRequest(HttpWebRequest httpWebRequest, HttpContextBase httpContext, string urlRefer)
        {
            var headers = new WebHeaderCollection();
            foreach (var key in httpContext.Request.Headers.AllKeys)
            {
                if (ModifiableHeader(key))
                {
                    headers[key] = httpContext.Request.Headers[key];
                }
            }

            headers["X-Forwarded-For"] = httpContext.Request.UserHostAddress;
            httpWebRequest.Headers = headers;
            httpWebRequest.ContentType = httpContext.Request.ContentType;
            httpWebRequest.Accept = httpContext.Request.Headers["Accept"];
            httpWebRequest.Referer = httpContext.Request.Headers["Referer"];
            httpWebRequest.UserAgent = httpContext.Request.UserAgent;
            httpWebRequest.ContentLength = httpContext.Request.ContentLength;
            httpWebRequest.ContentType = httpContext.Request.ContentType;
            httpWebRequest.Referer = urlRefer;
        }
    }
}
