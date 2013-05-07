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
    public interface IHtmlBlockProvider : ISiteElementProvider<HtmlBlock>, ILocalizableProvider<HtmlBlock>
    {
        void InitializeHtmlBlocks(Site site);

        void ExportHtmlBlocksToDisk(Site site);

        void Clear(Site site);

        void Export(IEnumerable<HtmlBlock> sources, System.IO.Stream outputStream);

        void Import(Site site, System.IO.Stream zipStream, bool @override);
    }
}
