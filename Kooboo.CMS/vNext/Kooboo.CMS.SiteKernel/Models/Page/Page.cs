#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Kooboo.CMS.SiteKernel.Models
{
    public partial class Page : ISiteObject, IInheritable<Page>, IIdentifiable, IPersistable
    {
        #region .ctor
        public Page() { }
        public Page(Site site, string fullName)
        {
            this.Site = site;
            this.FullName = fullName;
            this.Name = FullNameHelper.GetName(fullName);
        }
        public Page(Page parent, string name)
        {
            this.Site = parent.Site;
            this.FullName = FullNameHelper.Combine(parent.FullName, name);
            this.Name = name;
        }
        #endregion

        public Site Site
        {
            get;
            set;
        }
        public Page Parent
        {
            get
            {
                var parentFullName = FullNameHelper.GetParentFullName(this.FullName);
                if (!string.IsNullOrEmpty(parentFullName))
                {
                    return new Page(this.Site, parentFullName);
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    this.FullName = FullNameHelper.Combine(value.FullName, this.Name);
                }
                else
                {
                    this.FullName = this.Name;
                }
            }
        }

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

        #region IPersistable methods
        bool isDummy = true;
        bool IPersistable.IsDummy
        {
            get { return isDummy; }
        }

        void IPersistable.Init(IPersistable source)
        {
            this.isDummy = false;
            var page = (Page)source;
            this.Name = page.Name;
            this.Parent = page.Parent;
        }
        void IPersistable.OnSaved()
        {
        }

        void IPersistable.OnSaving()
        {
        }
        #endregion



    }
    public partial class Page
    {

        public string Name { get; set; }
        public string FullName { get; set; }
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

        public PageContent[] PageContents { get; set; }

        //private List<PagePosition> pagePositions = new List<PagePosition>();
        //[DataMember(Order = 20)]//
        //public List<PagePosition> PagePositions
        //{
        //    get { return this.pagePositions; }
        //    set { this.pagePositions = value; }
        //}

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
                return OutputCache != null && OutputCache.EnableCaching == true;
            }
        }
        private Dictionary<string, string> customFields;
        [DataMember(Order = 36)]
        public Dictionary<string, string> CustomFields
        {
            get
            {
                return customFields ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }
            set
            {
                if (value != null)
                {
                    customFields = new Dictionary<string, string>(value, StringComparer.OrdinalIgnoreCase);
                }
                else
                {
                    customFields = value;
                }
            }
        }

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
}
