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
using Kooboo.CMS.Sites.Models;
using System.IO;
using Kooboo.CMS.Sites.Models.Options;

namespace Kooboo.CMS.Sites.Persistence
{
    public class DomainMapping
    {
        public DomainMapping(string fullDomain, string userAgent, Site siteObject)
        {
            this.FullDomain = fullDomain;
            this.UserAgent = userAgent ?? "";
            this.SiteObject = siteObject;
        }
        public string FullDomain { get; set; }
        public string UserAgent { get; set; }
        public Site SiteObject { get; set; }
    }
    
    public interface ISiteProvider : ISiteElementProvider<Site>
    {
        [Obsolete("Move to PageManager")]
        Site GetSiteByHostNameNPath(string hostName, string requestPath);

        //Site GetSiteByHostName(string hostName);

        IEnumerable<DomainMapping> GetDomainTable();

        /// <summary>
        /// Alls the sites. Include all the child sites.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Site> AllSites();

        IEnumerable<Site> AllRootSites();

        IEnumerable<Site> ChildSites(Site site);


        void Offline(Site site);

        void Online(Site site);

        bool IsOnline(Site site);

        Site Create(Site parentSite, string siteName, Stream packageStream, CreateSiteOptions options);

        void Initialize(Site site);

        void Export(Site site, Stream outputStream, bool includeDatabase, bool includeSubSites);
    }
}
