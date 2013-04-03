using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;
using System.IO;

namespace Kooboo.CMS.Sites.Persistence
{
    public interface ISiteProvider : IProvider<Site>
    {
        Site GetSiteByHostNameNPath(string hostName, string requestPath);

        //Site GetSiteByHostName(string hostName);

        IDictionary<string, Site> GetDomainTable();

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

        Site Create(Site parentSite, string siteName, Stream packageStream, string repositoryName);

        void Initialize(Site site);

        void Export(Site site, Stream outputStream);
    }
}
