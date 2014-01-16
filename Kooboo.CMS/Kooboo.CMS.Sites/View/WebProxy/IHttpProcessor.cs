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
    using System.Text.RegularExpressions;

    public interface IHttpProcessor
    {
        string ProcessRequest(HttpContextBase httpContext, string url, string httpMethod, Func<string, bool, string> proxyUrlFunc);
    }
    [Dependency(typeof(IHttpProcessor))]
    public class HttpProcessor : IHttpProcessor
    {
        public virtual string ProcessRequest(HttpContextBase httpContext, string url, string httpMethod, Func<string, bool, string> proxyUrlFunc)
        {
            var httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);

            httpWebRequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            FillHttpRequest(httpWebRequest, httpContext, url);
            httpWebRequest.Method = httpMethod;
            httpWebRequest.AllowAutoRedirect = false;
            if (httpMethod.ToUpper() == "POST")
            {
                var requestStream = httpWebRequest.GetRequestStream();
                var data = httpContext.Request.InputStream.ReadData();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
            }
            var webResponse = httpWebRequest.GetResponse();
            CombineHttpHeaders(httpWebRequest, webResponse, httpContext);
            return ProcessWebResponse(webResponse, httpContext, proxyUrlFunc);

        }
        protected virtual void CombineHttpHeaders(HttpWebRequest httpWebRequest,
          WebResponse webResponse, HttpContextBase httpContext)
        {
            foreach (var key in webResponse.Headers.AllKeys)
            {
                if (key.ToUpper() == "SET-COOKIE")
                {
                    var value = Regex.Replace(webResponse.Headers[key], "domain=[^;]*;", "", RegexOptions.IgnoreCase);
                    httpContext.Response.AddHeader(key, value);
                }
                else
                {
                    if (ModifiableHeader(key))
                    {
                        httpContext.Response.AddHeader(key, webResponse.Headers[key]);
                    }
                }
            }          
        }
        protected virtual string ProcessWebResponse(WebResponse webResponse, HttpContextBase httpContext, Func<string, bool, string> proxyUrlFunc)
        {
            string result = "";
            if (webResponse is HttpWebResponse)
            {
                var httpWebResponse = (HttpWebResponse)webResponse;
                if (httpWebResponse.StatusCode == HttpStatusCode.Redirect || httpWebResponse.StatusCode == HttpStatusCode.MovedPermanently)
                {
                    string newUrl = httpWebResponse.Headers["Location"];
                    var proxyUrl = proxyUrlFunc(newUrl, false);

                    httpContext.Response.Redirect(proxyUrl);
                }
                else
                {
                    result = ProcessHttpWebResponse(httpWebResponse, httpContext);
                }
            }
            return result;
        }
        protected virtual string ProcessHttpWebResponse(HttpWebResponse httpWebResponse, HttpContextBase httpContext)
        {
            using (var responseStream = httpWebResponse.GetResponseStream())
            {
                var contentType = httpWebResponse.Headers["content-type"];
                // if the content-type is text/html, the response text will conbined with the Kooboo CMS page HTML.
                // or else will output the response stream directly.
                if (string.IsNullOrEmpty(contentType) || contentType.ToLower().Contains("text/html"))
                {
                    string Charset = httpWebResponse.CharacterSet;
                    Encoding encoding = Encoding.GetEncoding(Charset);
                    // read response
                    using (StreamReader sr =
                           new StreamReader(responseStream, encoding))
                    {
                        var html = sr.ReadToEnd();
                        return html;
                    }

                    //using (StreamReader loResponseStream = new StreamReader(responseStream))
                    //{
                    //    string html = loResponseStream.ReadToEnd();
                    //    return html;
                    //}
                }
                else
                {
                    httpContext.Response.RestoreRawOutput();
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
                case "IF-MODIFIED-SINCE":
                case "TRANSFER-ENCODING":
                //case "COOKIE":
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
