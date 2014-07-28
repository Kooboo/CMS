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

namespace Kooboo.CMS.SiteKernel.Models
{
    public class PageRoute
    {
        public static readonly PageRoute Default = new PageRoute();

        /// <summary>
        /// Gets or sets the link target.
        /// </summary>
        public LinkTarget? LinkTarget { get; set; }

        private string identifier;
        /// <summary>
        /// 页面的别名URL，通过对这个字段赋不同的值可以实现不同的URL展示方式：
        /// 1. 普通字符串，比如：detail，那就会带上父页面的virutal path，比如：news/detail
        /// 2. 以/开头，比如：/detail，此时返回的URL就会是：/detail
        /// 3. *，此时的它的virtual path会直接使用父页面的virtual path，只是查找方式会再多一些复杂的逻辑判断
        /// 4. 以#开头，此时可以满足一些特殊的URL需求，比如： news/{userKey}/detail， 此时的URL应该设置：#detail 
        /// 
        /// 关于*和#的使用约束，假设存在这样的一个URL地址：
        /// /travel-info/airport-transportation/*lodan/#airport/*date
        /// 那么，必须在对应的页面上必须要有相应的URLPath的设置才能正确解析和生成：
        /// 1. travel-info和airport-transportation没有特殊限制
        /// 2. lodan 的UrlPath必须设置：{UserKey}（一个变量）
        /// 3. airport的UrlPath必须设置：{UserKey} （也是一个变量）
        /// 4. date的UrlPath必须包含两个变量，并且将上一个#的Identifier带进来：{UserKey1}/airport/{UserKey2}
        /// </summary>
        public string Identifier
        {
            get { return identifier; }
            set
            {
                identifier = value;
                if (!string.IsNullOrEmpty(identifier) && identifier.StartsWith("#"))
                {
                    TrimmedIdentifier = identifier.Substring(1);
                }
            }
        }

        /// <summary>
        /// 去掉#的Identifier
        /// </summary>
        internal string TrimmedIdentifier { get; private set; }

        public string RoutePath { get; set; }

        public Dictionary<string, string> Defaults { get; set; }

        /// <summary>
        /// 用来给页面设置一个外部链接，如果这个值不为空，那页面在生成URL的时候会直接返回这个链接。
        /// </summary>
        public string ExternalUrl { get; set; }

        private string GetRouteUrl()
        {
            var routePath = RoutePath;
            if (!string.IsNullOrEmpty(TrimmedIdentifier))
            {
                if (!string.IsNullOrEmpty(routePath))
                {
                    routePath += "/" + TrimmedIdentifier;
                }
                else
                {
                    routePath = TrimmedIdentifier;
                }
            }
            return routePath;
        }
    }
}
