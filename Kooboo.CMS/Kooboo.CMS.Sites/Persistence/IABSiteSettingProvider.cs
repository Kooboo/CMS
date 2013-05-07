#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.ABTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Sites.Persistence
{
    public interface IABSiteSettingProvider : IProvider<ABSiteSetting>
    {
        void Export(IEnumerable<ABSiteSetting> sources, System.IO.Stream outputStream);

        void Import(System.IO.Stream zipStream, bool @override);
    }
}
