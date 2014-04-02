﻿#region License
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
    public interface ILayoutProvider : ISiteElementProvider<Layout>, ILocalizableProvider<Layout>
    {
        Models.Layout Copy(Site site, string sourceName, string destName);
        //IEnumerable<LayoutSample> AllSamples();
        //LayoutSample GetLayoutSample(string name);

        void Export(Site site, IEnumerable<Layout> sources, System.IO.Stream outputStream);

        void Import(Site site, System.IO.Stream zipStream, bool @override);
    }
}
