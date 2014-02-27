#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence
{
    public interface ILabelProvider : ISiteElementProvider<Label>
    {
        IEnumerable<string> GetCategories(Site site);

        IQueryable<Label> GetLabels(Site site, string category);

        void AddCategory(Site site, string category);

        void RemoveCategory(Site site, string category);

        void Export(Site site, IEnumerable<Label> labels, IEnumerable<string> categories, System.IO.Stream outputStream);

        void Import(Site site, System.IO.Stream zipStream, bool @override);

        void InitializeLabels(Site site);

        void ExportLabelsToDisk(Site site);

        void Flush(Site site);
    }
}
