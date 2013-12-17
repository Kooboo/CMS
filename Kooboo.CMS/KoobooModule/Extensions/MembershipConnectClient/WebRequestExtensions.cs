using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Kooboo.Web.Script.Serialization;
using Kooboo.Web.Url;

namespace Kooboo.CMS.VSExtensionTemplate.Extensions.MembershipConnectClient
{
    public class WebRequestExtensions
    {
        public static string Get(string url, Dictionary<string, string> dict)
        {
            url = dict.Aggregate(url, (current, param) => current.AddQueryParam(param.Key, param.Value));
            return Get(url);
        }

        public static string Get(string url)
        {
            using (var webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                return webClient.DownloadString(url);
            }
        }

        public static string Post(string url, Dictionary<string, string> dict)
        {
            var webRequest = WebRequest.Create(url) as HttpWebRequest;
            if (webRequest != null)
            {
                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                var postData = String.Join("&", dict.Select(item => item.Key + "=" + item.Value));
                using (var sw = new StreamWriter(webRequest.GetRequestStream()))
                {
                    sw.Write(postData);
                }

                using (var response = webRequest.GetResponse())
                {
                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
            throw new HttpException(500, "Get AccessToken failed!");
        }
    }
}
