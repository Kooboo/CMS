#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.ABTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Sites.Persistence
{
    public interface IABRuleSettingProvider : ISiteElementProvider<ABRuleSetting>
    {
        void Export(IEnumerable<ABRuleSetting> sources, System.IO.Stream outputStream);

        void Import(Site site,System.IO.Stream zipStream, bool @override);

    }
}
