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
using System.Web;
using System.IO;
using System.Text.RegularExpressions;

namespace Kooboo.Web.Url
{
    /// <summary>
    /// 
    /// </summary>
    public static class UrlUtility
    {
        /// <summary>
        /// 
        /// </summary>
        static Regex invalidUrlCharacter = new Regex(@"[^a-z|^_|^\d|^\u4e00-\u9fa5]+", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);

        /// <summary>
        /// To the URL string.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static string ToUrlString(this string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                return invalidUrlCharacter.Replace(s.Trim(), "-");
            }
            return s;
        }

        public static string UrlSeparatorChar = "/";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="virtualPaths">The virtual paths.<example>string[] {"path1","path2","path3"}</example></param>
        /// <returns> <value>path1/path2/path3</value> </returns>
        public static string Combine(params string[] virtualPaths)
        {
            if (virtualPaths.Length < 1)
                return null;

            return RawCombine(virtualPaths.Select(it => it.Contains("/") ? it : Uri.EscapeUriString(it)).ToArray());
        }

        /// <summary>
        /// Combines the specified virtual paths without Escape.
        /// </summary>
        /// <param name="virtualPaths">The virtual paths.</param>
        /// <returns></returns>
        public static string RawCombine(params string[] virtualPaths)
        {
            if (virtualPaths.Length < 1)
                return null;

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < virtualPaths.Length; i++)
            {
                var path = virtualPaths[i];
                if (String.IsNullOrEmpty(path))
                    continue;

                path = path.Replace("\\", "/");

                if (i > 0)
                {
                    // Not first one trim start '/'
                    path = path.TrimStart('/');
                    builder.Append("/");
                }
                if (i < virtualPaths.Length - 1)
                {
                    // Not last one trim end '/'
                    path = path.TrimEnd('/');
                }
                builder.Append(path);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Return full url start with http.
        /// </summary>
        /// <param name="relativeUrl">Url start with "~"</param>
        /// <returns></returns>
        public static string ToHttpAbsolute(string relativeUrl)
        {
            UriBuilder url = new UriBuilder(HttpContext.Current.Request.Url);
            var queryIndex = relativeUrl.IndexOf("?");
            if (queryIndex != -1 && queryIndex != relativeUrl.Length)
            {
                url.Query = relativeUrl.Substring(queryIndex + 1);
                relativeUrl = relativeUrl.Substring(0, queryIndex);
            }

            url.Path = VirtualPathUtility.ToAbsolute(relativeUrl);

            return url.Uri.AbsoluteUri.ToString();
        }

        /// <summary>
        /// Toes the HTTP absolute url.
        /// </summary>
        /// <param name="baseUri">The base URI. e.g: http://www.site.com</param>
        /// <param name="url">The URL. e.g: index?q=1</param>
        /// <returns></returns>
        public static string ToHttpAbsolute(string baseUri, string url)
        {
            url = ResolveUrl(url);
            if (string.IsNullOrEmpty(baseUri) || string.IsNullOrEmpty(url))
            {
                return url;
            }
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                return url;
            }

            if (!baseUri.StartsWith("http://") && !baseUri.StartsWith("https://"))
            {
                if (HttpContext.Current != null)
                {
                    baseUri = HttpContext.Current.Request.Url.Scheme + "://" + baseUri;

                    var _baseUri = new Uri(baseUri);

                    var isSSL = (bool?)HttpContext.Current.Items["IsSSL"];
                    if (isSSL != null)
                    {
                        UriBuilder uriBuilder = new UriBuilder(baseUri);
                        if (isSSL == true)
                        {
                            uriBuilder.Scheme = "https";
                            uriBuilder.Port = 443;
                        }
                        else
                        {
                            uriBuilder.Scheme = "http";
                        }
                        baseUri = uriBuilder.Uri.ToString();
                    }
                }
                else
                {
                    baseUri = "http://" + baseUri;
                }
            }

            return new Uri(new Uri(baseUri), url).ToString();
        }

        /// <summary>
        /// Equal to <see cref="System.Web.Mvc.UrlHelper.Content"/>  AND <see cref="System.Web.UI.Control.ResolveUrl"/>
        /// <remarks>
        /// Independent of HttpContext
        /// </remarks>
        /// </summary>
        /// <param name="relativeUrl">The URL. <example>~/a/b</example> </param>
        /// <returns><value>/a/b OR {virtualPath}/a/b</value></returns>
        public static string ResolveUrl(string relativeUrl)
        {
            if (!string.IsNullOrEmpty(relativeUrl) && HttpContext.Current != null && relativeUrl.StartsWith("~"))
            {
                //For FrontHttpRequestWrapper
                string applicationPath = HttpContext.Current.Items["ApplicationPath"] != null ? HttpContext.Current.Items["ApplicationPath"].ToString() : HttpContext.Current.Request.ApplicationPath;
                if (applicationPath == "/")
                {
                    return relativeUrl.Remove(0, 1);
                }
                else
                {
                    return applicationPath + relativeUrl.Remove(0, 1);
                }
            }
            return relativeUrl;
        }

        /// <summary>
        /// Wrap <see cref="System.Web.HttpServerUtilityBase.MapPath" />
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        /// <remarks>
        /// Independent of HttpContext
        /// </remarks>
        public static string MapPath(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return url;
            }
            var physicalPath = System.Web.Hosting.HostingEnvironment.MapPath(url);
            if (physicalPath == null)
            {
                physicalPath = url.TrimStart('~').Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar);

                physicalPath = Path.Combine(Kooboo.Settings.BaseDirectory, physicalPath);
            }
            return physicalPath;
        }

        /// <summary>
        /// Combines the query string.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="queries">The queries.</param>
        /// <returns></returns>
        public static string CombineQueryString(string baseUrl, params string[] queries)
        {
            string query = String.Join(String.Empty, queries).TrimStart('?');
            if (!String.IsNullOrEmpty(query))
            {
                if (baseUrl.Contains('?'))
                {
                    return baseUrl + query;
                }
                else
                {
                    return baseUrl + "?" + query;
                }
            }
            else
            {
                return baseUrl;
            }
        }

        /// <summary>
        /// Removes the query.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="names">The names.</param>
        /// <returns></returns>
        public static string RemoveQuery(string url, params string[] names)
        {
            string result = url;
            foreach (var each in names)
            {
                result = ReplaceQuery(url, each, String.Empty);
            }
            return result;
        }

        /// <summary>
        /// Replaces the query.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="name">The name.</param>
        /// <param name="newQuery">The new query.</param>
        /// <returns></returns>
        public static string ReplaceQuery(string url, string name, string newQuery)
        {
            return Regex.Replace(url, String.Format(@"&?\b{0}=[^&]*", name), newQuery, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Ensures the HTTP head.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static string EnsureHttpHead(string url)
        {
            if (String.IsNullOrEmpty(url))
                return url;

            if (!Regex.IsMatch(url, @"^\w"))
                return url;

            if (url.StartsWith("mailto:"))
                return url;

            if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                return url;

            return "http://" + url;
        }


        /// <summary>
        /// Gets the virtual path.
        /// </summary>
        /// <param name="physicalPath">The physical path.</param>
        /// <returns></returns>
        public static string GetVirtualPath(string physicalPath)
        {
            string rootpath = MapPath("~/");
            physicalPath = physicalPath.Replace(rootpath, "");
            physicalPath = physicalPath.Replace("\\", "/");
            return "~/" + physicalPath;
        }


        /// <summary>
        /// Adds the query param.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string AddQueryParam(
    this string source, string key, string value)
        {
            string delim;
            if ((source == null) || !source.Contains("?"))
            {
                delim = "?";
            }
            else if (source.EndsWith("?") || source.EndsWith("&"))
            {
                delim = string.Empty;
            }
            else
            {
                delim = "&";
            }

            return source + delim + HttpUtility.UrlEncode(key)
                + "=" + HttpUtility.UrlEncode(value);
        }

        /// <summary>
        /// Determines whether [is absolute URL] [the specified URL].
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>
        ///   <c>true</c> if [is absolute URL] [the specified URL]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAbsoluteUrl(string url)
        {
            return (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase)) || url.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
        }
    }
}
