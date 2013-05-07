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

namespace Kooboo.CMS.Sites.View.WebProxy
{
    public interface IProxyHtmlFixer
    {
        IHtmlString Fix(string baseUri, string html, Func<string, bool, string> proxyUrlFunc);
    }
    [Dependency(typeof(IProxyHtmlFixer))]
    public class ProxyHtmlFixer : IProxyHtmlFixer
    {
        #region Fix
        public IHtmlString Fix(string baseUri, string html, Func<string, bool, string> proxyUrlFunc)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            UpdateUrl(htmlDoc, baseUri, proxyUrlFunc);

            var bodyNode = GetBodyNode(htmlDoc);


            return new HtmlString(bodyNode.InnerHtml);
        }
        #endregion

        #region GetBodyNode
        protected virtual HtmlNode GetBodyNode(HtmlDocument htmlDoc)
        {
            var bodyNode = htmlDoc.CreateElement("body");

            var headNode = htmlDoc.DocumentNode.SelectSingleNode("//head");

            if (headNode != null)
            {
                var linkNodes = headNode.SelectNodes("//link[@href] | //script[@src]");
                bodyNode.AppendChildren(linkNodes);
            }

            var rawBodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");
            if (rawBodyNode == null)
            {
                bodyNode.AppendChild(htmlDoc.DocumentNode);
            }
            else
            {
                bodyNode.AppendChildren(rawBodyNode.ChildNodes);
            }
            return bodyNode;
        }
        #endregion

        #region UpdateUrl

        protected virtual void UpdateUrl(HtmlDocument htmlDoc, string baseUri, Func<string, bool, string> proxyUrlFunc)
        {
            var anchors_forms = htmlDoc.DocumentNode.SelectNodes("//a[@href] | //form[@action]");

            var links_scripts_images = htmlDoc.DocumentNode.SelectNodes("//link[@href] | //script[@src] | //img[@src]");

            if (proxyUrlFunc == null)
            {
                UpdateNodesUrl(anchors_forms, (url, isForm) => GenerateAbsoluteUrl(baseUri, url));
            }
            else
            {
                UpdateNodesUrl(anchors_forms, (url, isForm) => proxyUrlFunc(url, isForm));
            }

            UpdateNodesUrl(links_scripts_images, (url, isForm) => GenerateAbsoluteUrl(baseUri, url));
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
