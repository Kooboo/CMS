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
using System.Web.Routing;
using Kooboo.CMS.Sites.Models;
using System.Runtime.Serialization;
using Kooboo.Collections;
using Kooboo.Web.Url;
using System.IO;
using Kooboo.CMS.Sites.View;
using System.Collections.Specialized;
using System.Collections;
using Kooboo.Dynamic;
using Kooboo.CMS.Common.Persistence.Non_Relational;
namespace Kooboo.CMS.Sites.Models
{
    public enum PageType
    {
        Default,
        Static,
        Dynamic
    }
    public enum LinkTarget
    {
        _self,
        _blank,
        _parent,
        _top
    }

    [DataContract]
    public class PageRoute
    {
        public static readonly PageRoute Default = new PageRoute();

        /// <summary>
        /// Gets or sets the link target.
        /// </summary>
        [DataMember(Order = 5)]
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
        [DataMember(Order = 1)]
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

        [DataMember(Order = 2)]
        public string RoutePath { get; set; }

        [DataMember(Order = 3)]
        public Dictionary<string, string> Defaults { get; set; }

        /// <summary>
        /// 用来给页面设置一个外部链接，如果这个值不为空，那页面在生成URL的时候会直接返回这个链接。
        /// </summary>
        [DataMember(Order = 4)]
        public string ExternalUrl { get; set; }

        private Route mvcRoute;
        public Route ToMvcRoute()
        {
            if (mvcRoute == null)
            {
                var routePath = GetRouteUrl();
                mvcRoute = new Route(routePath, RouteValuesHelpers.GetRouteValues(Defaults), null);
            }
            return mvcRoute;

        }

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

        private ParsedRoute parsedRoute;
        internal ParsedRoute ToParsedRoute()
        {
            if (parsedRoute == null)
            {
                parsedRoute = RouteParser.Parse(GetRouteUrl());
            }
            return parsedRoute;
        }

    }
    [DataContract]
    public class Navigation : IComparable
    {
        [DataMember(Order = 1)]
        public bool Show { get; set; }
        [DataMember(Order = 3)]
        public string DisplayText { get; set; }
        [DataMember(Order = 5)]
        public int Order { get; set; }

        private bool? showInCrumb;
        [DataMember(Order = 7)]
        public bool? ShowInCrumb
        {
            get
            {
                if (showInCrumb == null)
                {
                    showInCrumb = true;
                }
                return showInCrumb;
            }
            set
            {
                showInCrumb = value;
            }
        }


        public override string ToString()
        {
            return DisplayText;
        }

        #region IComparable
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }
            var nav = (Navigation)obj;
            if (this.DisplayText == nav.DisplayText)
            {
                return 0;
            }
            if (string.IsNullOrEmpty(this.DisplayText))
            {
                return -1;
            }
            return this.DisplayText.CompareTo(nav.DisplayText);
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class PagePermission
    {
        /// <summary>
        /// <example>?,*,User1,Role1</example>
        /// </summary>
        /// <value>The allowed.</value>
        [DataMember(Order = 1)]
        public string Allowed { get; set; }
        [DataMember(Order = 3)]
        public string Denied { get; set; }
    }

    public static class PageHelper
    {
        public static string NameSplitter = "~";
        public static string CombineFullName(IEnumerable<string> pageNames)
        {
            return string.Join(NameSplitter, pageNames.Where(it => !string.IsNullOrEmpty(it)).ToArray());
        }
        public static IEnumerable<string> SplitFullName(string fullName)
        {
            return fullName.Split(new char[] { '~', '/' }, StringSplitOptions.RemoveEmptyEntries);
        }
        public static Page Parse(Site site, string fullName)
        {
            return new Page(site, SplitFullName(fullName).ToArray());
        }
    }

    #region Persistence
    [DataContract]
    public partial class Page : ISiteObject, IFilePersistable, IPersistable, IIdentifiable
    {
        public static Func<Site, Page, bool> IsLocalizeFunc = delegate(Site site, Page page)
        {
            return (new Page(site, page.PageNamePaths.ToArray())).Exists();
        };
        public Page()
        {

        }
        public Page(Page parent, string name)
            : base(parent.Site, name)
        {
            this.Parent = parent;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Page"/> class.
        /// </summary>
        /// <param name="site">The site.</param>
        /// <param name="pageNamePaths">The page name paths. <example>{"parent","child"}</example></param>
        public Page(Site site, params string[] pageNamePaths)
            : base(site, pageNamePaths.Last())
        {
            SetNamePath(pageNamePaths);
        }

        private void SetNamePath(IEnumerable<string> pageNamePaths)
        {
            this.Name = pageNamePaths.Last();
            if (pageNamePaths.Count() > 1)
            {
                this.Parent = new Page(Site, pageNamePaths.Take(pageNamePaths.Count() - 1).ToArray());
            }
        }
        public Page(Site site, string fullName) :
            this(site, PageHelper.SplitFullName(fullName).ToArray())
        {

        }
        public Page(string physicalPath)
            : base(physicalPath)
        {
        }
        [DataMember(Order = 2)]//
        public bool IsDefault { get; set; }

        private bool enableTheming = true;
        [DataMember(Order = 4)]//
        public bool EnableTheming
        {
            get
            {
                return enableTheming;
            }
            set
            {
                this.enableTheming = value;
            }
        }
        private bool enableScript = true;
        [DataMember(Order = 5)]//
        public bool EnableScript
        {
            get
            {
                return enableScript;
            }
            set
            {
                enableScript = value;
            }
        }
        [DataMember(Order = 6)]//
        public HtmlMeta HtmlMeta { get; set; }
        private PageRoute route = new PageRoute();
        [DataMember(Order = 7)]//
        public PageRoute Route
        {
            get
            {
                if (route == null)
                {
                    return PageRoute.Default;
                }
                return route;
            }
            set
            {
                route = value;
            }
        }
        Navigation navigation = new Navigation();
        [DataMember(Order = 8)]//
        public Navigation Navigation
        {
            get
            {
                return this.navigation;
            }
            set { this.navigation = value; }
        }
        PagePermission permission = new PagePermission();
        [DataMember(Order = 9)]//
        public PagePermission Permission
        {
            get { return this.permission; }
            set { this.permission = value; }
        }

        /// <summary>
        /// wrap for Layout
        /// </summary>
        /// <value>The name of the layout template.</value>
        [DataMember(Order = 10)]//
        public string Layout
        {
            get;
            set;
        }

        private List<PagePosition> pagePositions = new List<PagePosition>();
        [DataMember(Order = 20)]//
        public List<PagePosition> PagePositions
        {
            get { return this.pagePositions; }
            set { this.pagePositions = value; }
        }

        private List<DataRuleSetting> dataRules = new List<DataRuleSetting>();
        [DataMember(Order = 25)]//
        public List<DataRuleSetting> DataRules
        {
            get { return this.dataRules; }
            set
            {
                this.dataRules = value;
            }
        }

        private List<string> plugins = new List<string>();
        [DataMember(Order = 27)]//
        public List<string> Plugins
        {
            get
            {
                return plugins;
            }
            set
            {
                plugins = value;
            }
        }
        [DataMember(Order = 29)]
        public PageType PageType { get; set; }

        [DataMember(Order = 30)]
        public CacheSettings OutputCache { get; set; }

        public bool EnabledCache
        {
            get
            {
                return OutputCache != null && OutputCache.Duration > 0;
            }
        }
        [DataMember(Order = 36)]
        public Dictionary<string, string> CustomFields { get; set; }

        private bool? published;
        [DataMember(Order = 38)]
        public bool? Published
        {
            get
            {
                if (published == null)
                {
                    published = true;
                }
                return published;
            }
            set
            {
                published = value;
            }
        }

        [DataMember(Order = 39)]
        public string UserName { get; set; }

        [DataMember(Order = 40)]
        public string ContentTitle { get; set; }

        [DataMember(Order = 41)]
        public bool Searchable { get; set; }

        [DataMember]
        public bool RequireHttps { get; set; }

        [DataMember]
        public bool CacheToDisk { get; set; }
    }
    #endregion

    public partial class Page : DirectoryResource, IInheritable<Page>, Kooboo.CMS.Sites.Versioning.IVersionable
    {
        #region override PathResource

        string _fullName = null;
        /// <summary>
        /// 完整的页面名称，比如：Page1~Page2~Page3
        /// </summary>
        public virtual string FullName
        {
            get
            {

                _fullName = PageHelper.CombineFullName(PageNamePaths);

                return _fullName;
            }
            set
            {
                var names = PageHelper.SplitFullName(value);

                SetNamePath(names);
            }
        }

        /// <summary>
        /// 友好的页面名称，比如：Page1/Page2/Page3
        /// </summary>
        public string FriendlyName
        {
            get
            {
                return string.Join("/", PageNamePaths.ToArray());
            }
        }

        const string PATH_NAME = "Pages";
        /// <summary>
        /// 页面名称路径
        /// </summary>
        public virtual IEnumerable<string> PageNamePaths
        {
            get
            {
                if (this.Parent == null)
                {
                    return new string[] { this.Name };
                }
                return this.Parent.PageNamePaths.Concat(new string[] { this.Name }); ;
            }
        }
        /// <summary>
        /// 虚拟路径，用于生成页面URL的前缀，用页面完整名称和别号来生成，有一种特殊别名是：*
        /// 1. 如果有别名，则直接用别名来生成页面URL
        ///     1.1 如果别名是*，则去找父页面的虚拟路径来作为它的路径，一级一级往上找，直到找到不是*的父页面。
        ///     1.2 如果别名是以/开头，则将当直接当然绝对路径来处理，往上找父页面的路径来拼接
        ///     1.3 如果别名不是以/开头，则往上找所有的父页面的路径来拼接相对路径。
        /// 2. 如果没有设置别名，直接用页面的完整路径来生成URL
        /// </summary>
        public override string VirtualPath
        {
            get
            {
                var segment = this.Name;
                string virtualPath = "";
                if (Route != null && !string.IsNullOrEmpty(route.Identifier))
                {
                    if ((route.Identifier.StartsWith("#") || route.Identifier == "*") && Parent != null)
                    {
                        return Parent.LastVersion().AsActual().VirtualPath;
                    }
                    else if (route.Identifier.StartsWith("/"))
                    {
                        return route.Identifier;
                    }
                    else
                    {
                        if (Route != null && !string.IsNullOrEmpty(route.Identifier) && !route.Identifier.StartsWith("/"))
                        {
                            segment = route.Identifier;
                        }
                    }
                }
                if (this.Parent != null)
                {
                    virtualPath = UrlUtility.Combine(Parent.LastVersion().AsActual().VirtualPath, segment);
                }
                else
                {
                    virtualPath = segment;
                }
                if (!virtualPath.StartsWith("/"))
                {
                    virtualPath = "/" + virtualPath;
                }
                return virtualPath;
            }
        }
        public override IEnumerable<string> RelativePaths
        {
            get
            {
                return new string[] { PATH_NAME }.Concat(PageNamePaths.Take(PageNamePaths.Count() - 1));
            }
        }

        public override IEnumerable<string> ParseObject(IEnumerable<string> relativePaths)
        {
            this.Name = relativePaths.Last();
            var pathNameIndex = relativePaths.IndexOf(Page.PATH_NAME, StringComparer.InvariantCultureIgnoreCase);
            var count = relativePaths.Count();
            if (pathNameIndex + 2 < count)
            {
                this.Parent = new Page();
                return this.Parent.ParseObject(relativePaths.Take(count - 1));
            }
            return relativePaths.Take(pathNameIndex);
        }
        #endregion

        #region parent
        private Page _parent;
        public Page Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
                _fullName = null;
            }
        }

        public override Site Site
        {
            get
            {
                if (this.Parent != null)
                {
                    return Parent.Site;
                }
                return base.Site;
            }
            set
            {
                base.Site = value;
                if (this.Parent != null)
                {
                    this.Parent.Site = value;
                }
            }
        }
        #endregion

        #region IPersistable Members
        public string UUID
        {
            get
            {
                return this.FullName;
            }
            set
            {
                this.FullName = value;
            }
        }

        #region IDummy Members
        private bool isDummy = true;
        public bool IsDummy
        {
            get
            {
                return isDummy;
            }
            set
            {
                isDummy = value;
            }
        }

        #endregion

        void IPersistable.Init(IPersistable source)
        {
            var sourcePage = (Page)source;
            this.IsDummy = false;

            this.Name = sourcePage.Name;

            this.Site = sourcePage.Site;
            this.Parent = sourcePage.Parent;
        }
        void IPersistable.OnSaved()
        {
            this.IsDummy = false;
        }

        public string DataFileName
        {
            get
            {
                return SettingFileName;
            }
        }
        public string DataFile
        {
            get { return Path.Combine(this.PhysicalPath, DataFileName); }
        }
        public void OnSaving()
        {
        }
        #endregion

        #region Override object
        public static bool operator ==(Page obj1, Page obj2)
        {
            if (object.Equals(obj1, obj2) == true)
            {
                return true;
            }
            if (object.Equals(obj1, null) == true || object.Equals(obj2, null) == true)
            {
                return false;
            }
            return obj1.Equals(obj2);
        }
        public static bool operator !=(Page obj1, Page obj2)
        {
            return !(obj1 == obj2);
        }
        public override string ToString()
        {
            return this.FullName;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Page))
            {
                return false;
            }
            if (obj != null)
            {
                Page page = (Page)obj;
                if (this.Site == page.Site && page.FullName.EqualsOrNullEmpty(this.FullName, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            //The base.Equals method was overrided by PathResource
            return this.GetHashCode() == obj.GetHashCode();
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        #region IInheritable<Page> Members
        public virtual Page LastVersion()
        {
            return LastVersion(this.Site);
        }
        public virtual Page LastVersion(Site site)
        {
            do
            {
                var lastVersion = new Page(site, this.PageNamePaths.ToArray());
                if (lastVersion.Exists())
                {
                    return lastVersion;
                }
                site = site.Parent;
            } while (site != null);

            return null;
        }

        public virtual bool IsLocalized(Site site)
        {
            return IsLocalizeFunc(site, this);
        }

        public virtual bool HasParentVersion()
        {
            var parentSite = this.Site.Parent;
            while (parentSite != null)
            {
                var page = new Page(parentSite, this.FullName);
                if (page.Exists())
                {
                    return true;
                }
                parentSite = parentSite.Parent;
            }
            return false;
        }
        #endregion

        #region Link
        public string LinkText
        {
            get
            {
                if (this.Navigation != null && !string.IsNullOrEmpty(this.Navigation.DisplayText))
                {
                    return this.Navigation.DisplayText;
                }
                return this.Name;
            }
        }
        #endregion

        #region Exists

        public override bool Exists()
        {
            //  return File.Exists(this.DataFile); The page does not always store as file.
            return Kooboo.CMS.Sites.Persistence.Providers.PageProvider.Get(this) != null;
        }
        #endregion
    }
}
