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

namespace Kooboo.CMS.Sites.Persistence
{
    public interface IViewProvider : ISiteElementProvider<Kooboo.CMS.Sites.Models.View>, ILocalizableProvider<Kooboo.CMS.Sites.Models.View>
    {
        Models.View Copy(Site site, string sourceName, string destName);

        void Export(Site site, IEnumerable<Kooboo.CMS.Sites.Models.View> customErrors, System.IO.Stream outputStream);

        void Import(Site site, System.IO.Stream zipStream, bool @override);
    }
}
