#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Sites.DataRule;
using Kooboo.CMS.Sites.Extension.ModuleArea;
using Kooboo.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web.Routing;
namespace Kooboo.CMS.Sites.Models
{
    [DataContract(Name = "PagePosition")]
    [KnownTypeAttribute(typeof(PagePosition))]
    [KnownTypeAttribute(typeof(ViewPosition))]
    [KnownTypeAttribute(typeof(ModulePosition))]
    [KnownTypeAttribute(typeof(HtmlPosition))]
    [KnownType(typeof(ContentPosition))]
    [KnownTypeAttribute(typeof(HtmlBlockPosition))]
    [KnownTypeAttribute(typeof(ProxyPosition))]
    public abstract class PagePosition
    {
        private string pagePositionId = UniqueIdGenerator.GetInstance().GetBase32UniqueId(5);
        /// <summary>
        /// used to set the dynamic control id, also used to set the module url token.
        /// </summary>
        /// <value>The page position ID.</value>
        [DataMember(Order = 1)]
        public string PagePositionId
        {
            get { return pagePositionId; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    pagePositionId = UniqueIdGenerator.GetInstance().GetBase32UniqueId(5);
                }
                else
                {
                    pagePositionId = value;
                }
            }
        }
        [DataMember(Order = 2)]
        public string LayoutPositionId
        {
            get;
            set;
        }

        [DataMember(Order = 5)]
        public int Order { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is PagePosition))
            {
                return false;
            }
            if (obj != null)
            {
                var position = (PagePosition)obj;
                if (this.PagePositionId.EqualsOrNullEmpty(position.PagePositionId, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    [DataContract(Name = "ViewPosition")]
    [KnownTypeAttribute(typeof(ViewPosition))]
    public class ViewPosition : PagePosition
    {
        [DataMember(Order = 7)]
        public string ViewName { get; set; }

        private List<Parameter> parameters = new List<Parameter>();
        [DataMember(Order = 9)]//
        public List<Parameter> Parameters
        {
            get
            {
                return parameters;
            }
            set
            {
                parameters = value;
            }
        }

        [DataMember(Order = 10)]
        public CacheSettings OutputCache { get; set; }

        [DataMember]
        public bool SkipError { get; set; }

        public bool EnabledCache
        {
            get
            {
                return OutputCache != null && OutputCache.Duration > 0;
            }
        }
        public override string ToString()
        {
            //StringBuilder sb = new StringBuilder();

            //if (parameters != null)
            //{
            //    foreach (var item in parameters)
            //    {
            //        sb.AppendFormat("{0}:{1},", item.Name, item.Value);
            //    }
            //    if (sb.Length > 0)
            //    {
            //        sb.Remove(sb.Length - 1, 1);
            //    }
            //}

            var s = "View:" + this.ViewName;

            //if (sb.Length > 0)
            //{
            //    s = s + "(" + sb + ")";
            //}

            return s;
        }

        public IDictionary<string, object> ToParameterDictionary()
        {
            var parameters = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            if (this.Parameters != null)
            {
                foreach (var p in this.Parameters)
                {
                    parameters[p.Name] = p.Value;
                }
            }

            return parameters;
        }
    }

    [DataContract(Name = "ModulePosition")]
    [KnownTypeAttribute(typeof(ModulePosition))]
    public class ModulePosition : PagePosition
    {
        [DataMember(Order = 7)]
        public string ModuleName { get; set; }

        [DataMember(Order = 9)]
        public Entry Entry { get; set; }

        /// <summary>
        /// The page url with this module will exclude other module urls
        /// </summary>
        [DataMember(Order = 11)]
        public bool Exclusive { get; set; }

        [DataMember]
        public bool SkipError { get; set; }

        public override string ToString()
        {
            return "Module:" + this.ModuleName;
        }
    }

    [DataContract(Name = "HtmlPosition")]
    [KnownTypeAttribute(typeof(HtmlPosition))]
    public class HtmlPosition : PagePosition
    {
        [DataMember(Order = 7)]
        public string Html { get; set; }
        public override string ToString()
        {
            return "Html:" + this.Html.Ellipsis(10);
        }
    }

    public enum ContentPositionType
    {
        List,
        Detail
    }
    [DataContract(Name = "ContentPosition")]
    [KnownType(typeof(ContentPosition))]
    public class ContentPosition : PagePosition
    {
        [DataMember(Order = 7)]
        public IDataRule DataRule { get; set; }

        [DataMember(Order = 9)]
        public ContentPositionType Type { get; set; }

        public override string ToString()
        {
            return "ContentFolder:" + Type.ToString();
        }
    }


    [DataContract(Name = "HtmlBlockPosition")]
    [KnownTypeAttribute(typeof(HtmlBlockPosition))]
    public class HtmlBlockPosition : PagePosition
    {
        [DataMember(Order = 7)]
        public string BlockName { get; set; }

        public override string ToString()
        {
            return "Html block:" + BlockName;
        }

        public HtmlBlock GetHtmlBlock(Site site)
        {
            return new HtmlBlock(site, this.BlockName).LastVersion().AsActual();
        }
    }

    [DataContract(Name = "ProxyPosition")]
    [KnownTypeAttribute(typeof(ProxyPosition))]
    public class ProxyPosition : PagePosition
    {
        public ProxyPosition()
        {
            RequestPath = "/";
        }
        [DataMember]
        public string Host { get; set; }

        [DataMember]
        public string RequestPath { get; set; }

        [DataMember]
        public bool NoProxy { get; set; }


        [DataMember]
        public CacheSettings OutputCache { get; set; }

        public override string ToString()
        {
            return "Proxy:" + this.Host ?? "" + ":" + RequestPath ?? "/";
        }
    }
}
