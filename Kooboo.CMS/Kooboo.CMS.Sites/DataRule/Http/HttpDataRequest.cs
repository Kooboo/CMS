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
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Kooboo.CMS.Sites.DataRule.Http
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IHttpDataRequest))]
    public class HttpDataRequest : IHttpDataRequest
    {
        IResponseTextParser[] _parsers;
        public HttpDataRequest(IResponseTextParser[] parsers)
        {
            _parsers = parsers;
        }
        public dynamic GetData(string url, string httpMethod, string contentType, NameValueCollection form, NameValueCollection headers)
        {
            var responseText = SendRequest(url, httpMethod, ref contentType, form, headers);

            foreach (var parser in _parsers)
            {
                if (parser.Accept(responseText, contentType))
                {
                    return parser.Parse(responseText);
                }
            }
            return responseText;
        }

        private static string SendRequest(string url, string httpMethod, ref string contentType, NameValueCollection form, NameValueCollection headers)
        {
            contentType = (contentType ?? "application/x-www-form-urlencoded").ToLower();
            System.Net.ServicePointManager.Expect100Continue = false;

            CookieContainer cookies = new CookieContainer();
            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = httpMethod;
            webRequest.ContentType = contentType;
            webRequest.CookieContainer = cookies;

            var postBody = "";
            if (form != null)
            {
                if (httpMethod.ToUpper() == "GET")
                {
                    foreach (var key in form.AllKeys)
                    {
                        url = Kooboo.Common.Web.UrlUtility.AddQueryParam(url, key, form[key]);
                    }
                }
                else
                {
                    if (contentType.Contains("application/json"))
                    {
                        List<string> segments = new List<string>();

                        foreach (var key in form.AllKeys)
                        {
                            segments.Add(string.Format("'{0}':{1}", key, form[key]));
                        }
                        postBody = "{" + postBody + "}";
                    }
                    else
                    {
                        List<string> segments = new List<string>();

                        foreach (var key in form.AllKeys)
                        {
                            segments.Add(string.Format("{0}={1}", key, form[key]));
                        }
                        postBody = string.Join(",", segments);
                    }
                }
            }

            if (headers != null)
            {
                webRequest.Headers.Add(headers);
            }
            webRequest.ContentLength = postBody.Length;
            webRequest.UserAgent = "Kooboo CMS Data rule";
            webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            if (System.Web.HttpContext.Current != null)
            {
                webRequest.Referer = System.Web.HttpContext.Current.Request.Url.ToString();
            }
            if (httpMethod.ToUpper() == "POST")
            {
                StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream());
                requestWriter.Write(postBody);
                requestWriter.Close();
            }


            var response = webRequest.GetResponse();
            contentType = response.ContentType;
            StreamReader responseReader = new StreamReader(response.GetResponseStream());
            string responseData = responseReader.ReadToEnd();
            responseReader.Close();
            webRequest.GetResponse().Close();
            return responseData.Trim();
        }
    }

}
