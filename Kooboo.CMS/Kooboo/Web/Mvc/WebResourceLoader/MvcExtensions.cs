#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
// http://mvcresourceloader.codeplex.com/
using System;
using System.Collections;
using System.Collections.Generic;

using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Kooboo.Web.Mvc.WebResourceLoader.Configuration;


namespace Kooboo.Web.Mvc.WebResourceLoader
{
    internal delegate string ReferenceFomatter(string filename, string mimeType, string attributes);

    public static class MvcExtensions
    {
        public static IHtmlString ExternalResources(this HtmlHelper htmlHelper, string name)
        {
            return ExternalResources(htmlHelper, name, null);
        }

        public static IHtmlString ExternalResources(this HtmlHelper htmlHelper, string name, object htmlAttributes)
        {
            return ExternalResources(htmlHelper, name, new RouteValueDictionary(htmlAttributes));
        }
        public static IHtmlString ExternalResources(this HtmlHelper htmlHelper, string name, RouteValueDictionary htmlAttributes)
        {
            return ExternalResources(htmlHelper, AreaHelpers.GetAreaName(htmlHelper.ViewContext.RouteData), name, htmlAttributes);
        }
        public static IHtmlString ExternalResources(this HtmlHelper htmlHelper, string areaName, string name, RouteValueDictionary htmlAttributes)
        {
            return ExternalResources(htmlHelper, areaName, name, htmlAttributes, null);
        }
        public static IHtmlString ExternalResources(this HtmlHelper htmlHelper, string areaName, string name, RouteValueDictionary htmlAttributes, string baseUri)
        {
            ReferenceElement settings = GetSettings(areaName, name);

            // get distinct show if / hide if conditions and group into files
            IDictionary<Condition, IList<FileInfoElement>> conditions = new Dictionary<Condition, IList<FileInfoElement>>(new ConditionComparer());
            foreach (FileInfoElement fileInfo in settings.Files)
            {
                Condition condition = new Condition { If = fileInfo.If };
                if (conditions.ContainsKey(condition))
                {
                    conditions[condition].Add(fileInfo);
                }
                else
                {
                    conditions.Add(condition, new List<FileInfoElement> { fileInfo });
                }
            }

            string attributes = CreateAttributeList(htmlAttributes);

            switch (settings.MimeType)
            {
                case "text/x-javascript":
                case "text/javascript":
                case "text/ecmascript":
                    {
                        ReferenceFomatter formatter = (filename, mimeType, attribs) => string.Format("<script src=\"{0}\" type=\"{1}\"{2}></script>", filename, settings.MimeType, attribs);
                        return OutputReferences(htmlHelper.ViewContext, conditions, settings, attributes, formatter, areaName, baseUri);
                    }
                case "text/css":
                    {
                        ReferenceFomatter formatter = (filename, mimeType, attribs) => string.Format("<link rel=\"Stylesheet\" href=\"{0}\" type=\"{1}\"{2} />", filename, settings.MimeType, attribs);
                        return OutputReferences(htmlHelper.ViewContext, conditions, settings, attributes, formatter, areaName, baseUri);
                    }

                // TODO: Decide any other reference types that we want to handle e.g. images
                default:
                    {
                        return new HtmlString(string.Empty);
                    }
            }
        }

        private static IHtmlString OutputReferences(ViewContext viewContext, IDictionary<Condition, IList<FileInfoElement>> conditions, ReferenceElement settings, string attributes, ReferenceFomatter formatter, string areaName, string baseUrl)
        {
            var section = ConfigurationManager.GetSection(areaName);
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<Condition, IList<FileInfoElement>> keyValuePair in conditions)
            {
                Condition condition = keyValuePair.Key;
                IList<FileInfoElement> files = keyValuePair.Value;

                // we need to wrap these files in conditional comments if the condition is anything but Action = Show with empty if (i.e. show to everything)
                bool conditionalCommentRequired = !string.IsNullOrEmpty(condition.If);

                if (conditionalCommentRequired)
                {
                    sb.AppendFormat("<!--[if {0}]>", condition.If);
                    sb.AppendLine();
                }

                switch (section.Mode)
                {
                    case Mode.Release:
                        UrlHelper urlHelper = new UrlHelper(viewContext.RequestContext);

                        //VirtualPathData virtualPathData = RouteTable.Routes.GetVirtualPath(viewContext.RequestContext,
                        //    new RouteValueDictionary(new
                        //    {
                        //        area = viewContext.RouteData.Values["area"],
                        //        controller = "WebResource",
                        //        action = "Index",
                        //        name = settings.Name,
                        //        version = section.Version,
                        //        condition = condition.If
                        //    }));
                        string url = urlHelper.Action("Index", new
                        {
                            area = areaName,
                            controller = "WebResource",
                            action = "Index",
                            name = settings.Name,
                            version = section.Version,
                            condition = condition.If
                        });
                        if (!string.IsNullOrEmpty(baseUrl))
                        {
                            url = Kooboo.Web.Url.UrlUtility.ToHttpAbsolute(baseUrl, url);
                        }
                        sb.AppendLine(formatter.Invoke(url, settings.MimeType, attributes));
                        break;
                    case Mode.Debug:
                        foreach (FileInfoElement fileInfo in files)
                        {
                            string fileUrl = VirtualPathUtility.ToAbsolute(fileInfo.Filename);
                            if (!string.IsNullOrEmpty(baseUrl))
                            {
                                fileUrl = Kooboo.Web.Url.UrlUtility.ToHttpAbsolute(baseUrl, fileUrl);
                            }
                            sb.AppendLine(formatter.Invoke(fileUrl, settings.MimeType, attributes));
                        }
                        break;
                }

                if (conditionalCommentRequired)
                {
                    sb.AppendFormat("<![endif]-->");
                    sb.AppendLine();
                }
            }

            return new HtmlString(sb.ToString());
        }

        private static string CreateAttributeList(RouteValueDictionary attributes)
        {
            StringBuilder builder = new StringBuilder();
            if (attributes != null)
            {
                foreach (string str in attributes.Keys)
                {
                    string str2 = attributes[str].ToString();
                    if (attributes[str] is bool)
                    {
                        str2 = str2.ToLowerInvariant();
                    }
                    builder.AppendFormat(" {0}=\"{1}\"", str.ToLowerInvariant().Replace("_", ""), str2);
                }
            }
            return builder.ToString();
        }

        private static ReferenceElement GetSettings(string areaName, string name)
        {
            var section = ConfigurationManager.GetSection(areaName);
            ReferenceElement settings = section.References[name];
            if (settings == null)
                throw new WebResourceException(string.Format("Web resource name {0} not found", name));
            return settings;
        }

        public static IEnumerable<string> GetFiles(string areaName, string configName)
        {
            var setting = GetSettings(areaName, configName);
            foreach (FileInfoElement fileInfo in setting.Files)
            {
                yield return fileInfo.Filename;
            }
        }
    }
    public class Condition
    {
        public string If { get; set; }
        public override bool Equals(object obj)
        {
            var condition = (Condition)obj;
            return this.If.Equals(condition.If);
        }
        public override int GetHashCode()
        {
            return this.If.GetHashCode();
        }
    }
    internal class ConditionComparer : IEqualityComparer<Condition>
    {
        public bool Equals(Condition x, Condition y)
        {
            return x.If.Equals(y.If);
        }

        public int GetHashCode(Condition obj)
        {
            return obj.GetHashCode();
        }
    }
}
