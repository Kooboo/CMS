#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.SiteKernel.Models;
using Kooboo.CMS.SiteKernel.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.Services
{
    public class SiteService : ServiceBase<Site, ISiteProvider>, ISiteService
    {
        #region .ctor
        public SiteService(ISiteProvider siteProvider)
            : base(siteProvider)
        {

        }
        #endregion

        public Site Create(Site parentSite, string siteName, System.IO.Stream packageStream)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Site> RootSites()
        {
            return Provider.RootSites();
        }

        public IEnumerable<Site> ChildSites(Site parentSite)
        {
            return Provider.ChildSites(parentSite);
        }

        public void Import(Site data, System.IO.Stream zipData, bool @override)
        {
            throw new NotImplementedException();
        }

        public System.IO.Stream[] Export(IEnumerable<Site> data)
        {
            throw new NotImplementedException();
        }       
    }
}
