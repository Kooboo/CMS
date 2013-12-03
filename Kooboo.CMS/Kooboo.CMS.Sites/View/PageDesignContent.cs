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
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using Kooboo.Web.Script.Serialization;
namespace Kooboo.CMS.Sites.View
{
    using Kooboo.Extensions;
    using Kooboo.CMS.Sites.Models;
    using Kooboo.CMS.Sites.DataRule;
    using Kooboo.CMS.Common;

    public abstract class PageDesignContent : PageDesignHtml
    {
        public static string Code(string input)
        {
            return HttpUtility.HtmlAttributeEncode(input);
        }

        public PageDesignContent(PagePosition pos)
            : base()
        {
            if (pos == null) { throw new ArgumentNullException(); }

            if (string.IsNullOrEmpty(pos.PagePositionId))
            {
                //pos.PagePositionId = Guid.NewGuid().ToString();
                pos.PagePositionId = UniqueIdGenerator.GetInstance().GetBase32UniqueId(5);
            }

            this.Position = pos;

            //this.TagName = "li";

            this.ClassName = "pagedesign-content";

            this.Parameter.Add("Name", PageDesignContent.Code(this.ToString()));

            this.Parameter.Add("PagePositionId", PageDesignContent.Code(pos.PagePositionId));

            this.Parameter.Add("Order", pos.Order.ToString());

            this.Parameter.Add("Type", this.GetTypeKey());
        }

        public PagePosition Position
        {
            get;
            private set;
        }

        public abstract string GetTypeKey();

        public override string ToString()
        {
            return this.Position.ToString();
        }
    }

    public class PageDesignViewContent : PageDesignContent
    {
        public PageDesignViewContent(ViewPosition pos)
            : base(pos)
        {
            this.Parameter.Add("ViewName", PageDesignContent.Code(pos.ViewName));
            this.Parameter.Add("SkipError", pos.SkipError.ToString().ToLower());
            if (pos.OutputCache != null)
            {
                var outputCacheJson = new
                {
                    Duration = pos.OutputCache.Duration,
                    ExpirationPolicy = pos.OutputCache.ExpirationPolicy.ToString()
                };
                var serializer = new JavaScriptSerializer();
                var outputCacheJsonString = serializer.Serialize(outputCacheJson);
                this.Parameter.Add("OutputCache", PageDesignContent.Code(outputCacheJsonString));
            }
            if (pos.Parameters != null && pos.Parameters.Count > 0)
            {
                var parameters = new List<object>();
                pos.Parameters.ForEach((p) =>
                {
                    var value = string.Empty;
                    if (p.Value != null)
                    {
                        value = p.Value.ToString();
                        if (p.DataType == DataType.DateTime)
                        {
                            var date = ((DateTime)p.Value);
                            value = date.ToLocalTime().ToShortDateString();
                        }
                    }
                    parameters.Add(new
                    {
                        Name = p.Name,
                        DataType = p.DataType.ToString(),
                        Value = value
                    });
                });
                var serializer = new JavaScriptSerializer();
                var parametersJson = serializer.Serialize(parameters);
                this.Parameter.Add("Parameters", PageDesignContent.Code(parametersJson));
            }
        }

        public const string TypeKey = "view";

        public override string GetTypeKey() { return TypeKey; }
    }

    public class PageDesignModuleContent : PageDesignContent
    {
        public PageDesignModuleContent(ModulePosition pos)
            : base(pos)
        {
            this.Parameter.Add("ModuleName", PageDesignContent.Code(pos.ModuleName));
            this.Parameter.Add("Exclusive", pos.Exclusive.ToString().ToLower());
            this.Parameter.Add("SkipError", pos.SkipError.ToString().ToLower());
            if (pos.Entry != null)
            {
                this.Parameter["LinkToEntryName"] = pos.Entry.LinkToEntryName;
                this.Parameter["EntryName"] = pos.Entry.Name;
                this.Parameter.Add("EntryAction", pos.Entry.Action);
                this.Parameter.Add("EntryController", pos.Entry.Controller);
                this.Parameter.Add("Values", pos.Entry.Values == null ? "[]" : pos.Entry.Values.ToList().ToJSON());
            }
        }

        public const string TypeKey = "module";

        public override string GetTypeKey() { return TypeKey; }
    }

    public class PageDesignHtmlContent : PageDesignContent
    {
        public PageDesignHtmlContent(HtmlPosition pos)
            : base(pos)
        {
            // html
            this.Parameter.Add("Html", PageDesignContent.Code(pos.Html));

            // name
            var name = HttpUtility.UrlDecode(pos.Html).StripAllTags().Trim();
            name = System.Text.RegularExpressions.Regex.Replace(name, "\\s+", " ");
            if (name.Length > 10) { name = name.Substring(0, 7) + "..."; }
            this.Parameter["Name"] = PageDesignContent.Code("Html:" + name);
        }

        public const string TypeKey = "html";

        public override string GetTypeKey() { return TypeKey; }
    }

    public class PageDesignFolderContent : PageDesignContent
    {
        public PageDesignFolderContent(ContentPosition pos)
            : base(pos)
        {
            this.Parameter.Add("ContentPositionType", pos.Type.ToString());
            var dataRule = pos.DataRule as FolderDataRule;
            if (dataRule != null)
            {
                var dataRuleJson = new
                {
                    FolderName = dataRule.FolderName,
                    Top = dataRule.Top,
                    WhereClauses = dataRule.WhereClauses
                };
                var serializer = new JavaScriptSerializer();
                var dataRuleJsonString = serializer.Serialize(dataRuleJson);
                this.Parameter.Add("DataRule", PageDesignContent.Code(dataRuleJsonString));
            }
        }

        public const string TypeKey = "folder";

        public override string GetTypeKey() { return TypeKey; }
    }

    public class PageDesignHtmlBlockContent : PageDesignContent
    {
        public PageDesignHtmlBlockContent(HtmlBlockPosition pos, string body)
            : base(pos)
        {
            this.Parameter["BlockName"] = pos.BlockName;
            this.Attribute["Html"] = PageDesignContent.Code(body);
        }

        public const string TypeKey = "htmlBlock";

        public override string GetTypeKey() { return TypeKey; }
    }

    public class PageDesignProxyContent : PageDesignContent
    {
        public PageDesignProxyContent(ProxyPosition pos)
            : base(pos)
        {
            this.Parameter["Host"] = pos.Host;
            this.Parameter["RequestPath"] = pos.RequestPath;
            this.Parameter["NoProxy"] = pos.NoProxy.ToString().ToLower();
            if (pos.OutputCache != null)
            {
                var outputCacheJson = new
                {
                    Duration = pos.OutputCache.Duration,
                    ExpirationPolicy = pos.OutputCache.ExpirationPolicy.ToString()
                };
                var serializer = new JavaScriptSerializer();
                var outputCacheJsonString = serializer.Serialize(outputCacheJson);
                this.Parameter.Add("OutputCache", PageDesignContent.Code(outputCacheJsonString));
            }
        }

        public const string TypeKey = "proxy";

        public override string GetTypeKey() { return TypeKey; }
    }
}
