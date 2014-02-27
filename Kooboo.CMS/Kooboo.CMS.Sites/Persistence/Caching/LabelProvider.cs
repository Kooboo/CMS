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

namespace Kooboo.CMS.Sites.Persistence.Caching
{
    public class LabelProvider : ProviderBase<Label>, ILabelProvider
    {
        #region .ctor
        private ILabelProvider inner;
        public LabelProvider(ILabelProvider inner)
            : base(inner)
        {
            this.inner = inner;
        }
        #endregion

        public IEnumerable<string> GetCategories(Site site)
        {
            return inner.GetCategories(site);
        }

        public IQueryable<Label> GetLabels(Site site, string category)
        {
            return inner.GetLabels(site, category);
        }

        public void AddCategory(Site site, string category)
        {
            inner.AddCategory(site, category);
        }

        public void RemoveCategory(Site site, string category)
        {
            inner.RemoveCategory(site, category);
            ClearObjectCache(site);
        }

        public void Export(Site site, IEnumerable<Label> labels, IEnumerable<string> categories, System.IO.Stream outputStream)
        {
            inner.Export(site, labels, categories, outputStream);
        }

        public void Import(Site site, System.IO.Stream zipStream, bool @override)
        {
            inner.Import(site, zipStream, @override);
            ClearObjectCache(site);
        }

        public void InitializeLabels(Site site)
        {
            inner.InitializeLabels(site);
        }

        public void ExportLabelsToDisk(Site site)
        {
            inner.ExportLabelsToDisk(site);
        }

        public IEnumerable<Label> All(Site site)
        {
            return inner.All(site);
        }

        public IEnumerable<Label> All()
        {
            return inner.All();
        }

        protected override string GetItemCacheKey(Label o)
        {
            return string.Format("Label:category:{0}:name:{1}", o.Category, o.Name);
        }


        public void Flush(Site site)
        {
            try
            {
                this.inner.Flush(site);
            }
            finally
            {
                ClearObjectCache(site);
            }
            
            
        }
    }
}
