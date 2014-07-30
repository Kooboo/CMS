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

namespace Kooboo.CMS.SiteKernel.Services.Site
{
    public class SiteService : ISiteService
    {
        public IEnumerable<Site> RootSites()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Site> ChildSites(Site parentSite)
        {
            throw new NotImplementedException();
        }

        public Site Get(Site o)
        {
            throw new NotImplementedException();
        }

        public void Add(Site o)
        {
            throw new NotImplementedException();
        }

        public void Update(Site @new, Site old)
        {
            throw new NotImplementedException();
        }

        public void Delete(Site o)
        {
            throw new NotImplementedException();
        }

        public void Import(Site data, byte[] zipData, bool @override)
        {
            throw new NotImplementedException();
        }

        public byte[] Export(IEnumerable<Site> data)
        {
            throw new NotImplementedException();
        }
    }
}
