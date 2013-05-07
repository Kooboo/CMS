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
using Kooboo.CMS.Sites.Globalization;

namespace Kooboo.CMS.Sites.Providers.AzureTable.LabelProvider
{
    public class RepositoryFactory : IRepositoryFactory
    {
        public Kooboo.Globalization.IElementRepository CreateRepository(Models.Site site)
        {
            return new LabelRepository(site.FullName);
        }
    }
}
