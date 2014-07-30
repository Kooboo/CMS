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
        }
        public Page(Page parent, string name)
        {
            this.Site = parent.Site;
            this.FullName = FullNameHelper.Combine(parent.FullName, name);
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
        }

        public string UUID
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.Name = value;
            }
        }

        #region IPersistable methods
        public bool IsDummy
        {
            get { throw new NotImplementedException(); }
        }

        public void Init(IPersistable source)
        {
            throw new NotImplementedException();
        }
        public void OnSaved()
        {
            throw new NotImplementedException();
        }

        public void OnSaving()
        {
            throw new NotImplementedException();
        }
        #endregion


      
    }
    public partial class Page
    {

        public string Name { get; set; }
        public string FullName { get; set; }

        public bool IsDefault { get; set; }

        public string Layout { get; set; }

        public PageType PageType { get; set; }

        public bool EnableTheming { get; set; }

        public bool EnableScript { get; set; }

        public bool Published { get; set; }

        public string Editor { get; set; }

        public bool RequireHttps { get; set; }

        public HtmlMeta HtmlMeta { get; set; }

        public PageRoute Route { get; set; }

        public Navigation Navigation { get; set; }

        public PagePermission Permission { get; set; }

        public string[] PluginTypes { get; set; }

        public PageContent[] PageContents { get; set; }

        public DataRuleSetting[] DataRules { get; set; }

        public CacheSettings OutputCache { get; set; }

        public Dictionary<string, string> CustomFields { get; set; }
    }
}
