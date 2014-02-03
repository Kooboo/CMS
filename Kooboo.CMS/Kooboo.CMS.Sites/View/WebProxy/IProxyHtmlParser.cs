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
using HtmlAgilityPack;
using Kooboo.CMS.Common.Runtime.Dependency;
using System.Text.RegularExpressions;
using System.IO;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.View.PositionRender;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.View.WebProxy
{
    public interface IProxyHtmlParser
    {
        IHtmlString Parse(ProxyRenderContext proxyRenderContext, string rawHtml
            , Func<string, bool, string> proxyUrlFunc);
    }
    [Dependency(typeof(IProxyHtmlParser))]
    public class ProxyHtmlParser : IProxyHtmlParser
    {
        #region Parse
        public IHtmlString Parse(ProxyRenderContext proxyRenderContext, string rawHtml
            , Func<string, bool, string> proxyUrlFunc)
        {
            var stylesheets = new List<string>();

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(rawHtml);

            HtmlNode headNode = null;
            HtmlNode bodyNode = null;

            foreach (var node in htmlDoc.DocumentNode.Descendants().ToArray())
            {
                //if (IsStylesheet(node) && proxyRenderContext.ProxyPosition.ProxyStylesheet == true)
                //{
                //    stylesheets.Add(node.Attributes["href"].Value);
                //    node.Remove();
                //    continue;
                //}
                var nodeName = node.Name.ToLower();
                var elementName = node.Attributes["name"] == null ? "" : node.Attributes["name"].Value.ToLower();
                if (elementName == "__viewstate") { continue; }
                if (elementName == "__eventvalidation") { continue; }
                if (nodeName == "head")
                {
                    headNode = node;
                }
                if (nodeName == "body")
                {
                    bodyNode = node;
                }
                foreach (var attr in node.Attributes)
                {
                    if (IsW3CUrlAttribute(attr.Name.ToLower()) || attr.Value.StartsWith("/"))
                    {
                        var isStaticResource = IsStaticResource(node, attr);
                        if (isStaticResource)
                        {
                            attr.Value = GenerateAbsoluteUrl(proxyRenderContext.ProxyPosition.HostUri.ToString(), attr.Value);
                        }
                        else
                        {
                            var isForm = node.Name.ToLower() == "form";
                            attr.Value = proxyUrlFunc(attr.Value, isForm);
                        }
                    }
                }
            }

            string newHtml = "";
            if (bodyNode == null)
            {
                newHtml = htmlDoc.DocumentNode.InnerHtml;
            }
            else
            {
                if (headNode != null)
                {
                    newHtml = headNode.InnerHtml;
                }
                newHtml = newHtml + bodyNode.InnerHtml;
            }

            string injectScript = "";
            if (proxyRenderContext.PageRequestContext != null)
            {
                injectScript = InjectScriptForAjax(proxyRenderContext.PageRequestContext.Site, proxyRenderContext.PageRequestContext.Page);
                newHtml = newHtml + injectScript;
            }

            //if (proxyRenderContext.ProxyPosition.ProxyStylesheet == true && stylesheets.Count > 0)
            //{
            //    newHtml = ProxyStyleSheet(proxyRenderContext, stylesheets, newHtml);
            //}

            return new HtmlString(newHtml);
        }
        #endregion

//        private string ProxyStyleSheet(ProxyRenderContext proxyRenderContext, IEnumerable<string> stylesheets, string html)
//        {
//            var styles = string.Join(",", stylesheets.ToArray());
//            var urlHelper = new UrlHelper(proxyRenderContext.ControllerContext.RequestContext);
//            var proxyStyleURL = urlHelper.Action("ProxyStyle", "Resource", new
//            {
//                SiteName = proxyRenderContext.PageRequestContext.Site,
//                PageName = proxyRenderContext.PageRequestContext.Page.FullName,
//                PositionId = proxyRenderContext.ProxyPosition.PagePositionId,
//                Styles = styles
//            });

//            return string.Format(@"<div id='{0}'><link href='{1}' rel='stylesheet' type='text/less'/> <script src='{3}' type='text/javascript'></script>
//            {2}
//            </div>", proxyRenderContext.ProxyPosition.PagePositionId, proxyStyleURL, html, Kooboo.Web.Url.UrlUtility.ResolveUrl("~/Scripts/less.js"));
//        }
        //#region IsStylesheet
        //private bool IsStylesheet(HtmlNode node)
        //{
        //    if (node.Name.ToLower() == "link")
        //    {
        //        var rel = node.Attributes["rel"];
        //        if (rel != null && !string.IsNullOrEmpty(rel.Value) && rel.Value.ToLower() == "stylesheet")
        //        {
        //            var href = node.Attributes["href"];
        //            if (href != null && !string.IsNullOrEmpty(href.Value) && !href.Value.Contains(";"))
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}
        //#endregion

        #region InjectScriptForAjax
        private string InjectScriptForAjax(Site site, Page page)
        {
            string query = string.Format("hasRemoteProxy=true&cms_siteName={0}&cms_pageName={1}", site.FullName, page.FullName);
            return string.Format(@"<script>(function (open){{
        XMLHttpRequest.prototype.open = function (method, url, async, user, pass){{
            if (url.indexOf('?') == -1) {{
                url = url + '?{0}';
            }}
            else {{
                url = url + '&{0}';
            }}
            open.call(this, method, url, async, user, pass);
        }};
    }})(XMLHttpRequest.prototype.open);</script>", query);
        }
        #endregion

        #region UpdateUrl
        private bool IsStaticResource(HtmlNode node, HtmlAttribute attr)
        {
            var nodeName = node.Name.ToLower();
            if (nodeName == "link" || nodeName == "script" || nodeName == "img")
            {
                return true;
            }
            var attValue = (attr.Value ?? "").ToLower();
            if (attr.Value.StartsWith("#") || attr.Value.StartsWith("javascript:"))
            {
                return true;
            }

            if (attValue.EndsWith(".js") || attValue.EndsWith(".css") || attValue.EndsWith(".png") || attValue.EndsWith(".jpg")
                || attValue.EndsWith(".jpeg") || attValue.EndsWith(".ico") || attValue.EndsWith(".gif"))
            {
                return true;
            }

            return false;
        }
        private static string GenerateAbsoluteUrl(string baseUri, string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return baseUri;
            }

            if (!url.StartsWith("#") && !url.StartsWith("javascript:"))
            {
                return new Uri(new Uri(baseUri), url).ToString();
            }
            else
            {
                return url;
            }
        }
        private void UpdateNodesUrl(IEnumerable<HtmlNode> htmlNodes, Func<string, bool, string> urlGenerator)
        {
            if (htmlNodes != null)
            {
                foreach (var item in htmlNodes)
                {
                    var rawUrl = GetUrl(item);
                    var url = urlGenerator(rawUrl, item.Name.ToLower() == "form");
                    SetUrl(item, url);
                }
            }
        }
        private bool IsW3CUrlAttribute(string attrName)
        {
            return attrName == "href" || attrName == "action" || attrName == "src";
        }

        private string GetUrl(HtmlNode htmlNode)
        {
            switch (htmlNode.Name.ToLower())
            {
                case "a":
                case "link":
                    return htmlNode.Attributes["href"].Value;
                case "form":
                    return htmlNode.Attributes["action"].Value;
                case "script":
                case "img":
                    return htmlNode.Attributes["src"].Value;
                default:
                    return "";
            }
        }
        private void SetUrl(HtmlNode htmlNode, string url)
        {
            switch (htmlNode.Name.ToLower())
            {
                case "a":
                case "link":
                    htmlNode.Attributes["href"].Value = url;
                    break;
                case "form":
                    htmlNode.Attributes["action"].Value = url;
                    break;
                case "script":
                case "img":
                    htmlNode.Attributes["src"].Value = url;
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
