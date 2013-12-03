#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.CMS.Sites.Versioning;
using Kooboo.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Kooboo.CMS.Sites.Services
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(LayoutManager))]
    public class LayoutManager : PathResourceManagerBase<Layout, ILayoutProvider>
    {
        #region .ctor
        public LayoutManager(ILayoutProvider provider) : base(provider) { }
        #endregion

        #region Add
        public override void Add(Site site, Layout o)
        {
            base.Add(site, o);
            VersionManager.LogVersion(o);
        }
        #endregion

        #region Get
        public override Layout Get(Site site, string name)
        {
            return Provider.Get(new Layout(site, name));
        }
        #endregion

        #region Update
        public override void Update(Site site, Layout @new, Layout old)
        {
            base.Update(site, @new, old);
            VersionManager.LogVersion(@new);
        }
        #endregion

        #region Remove
        public override void Remove(Site site, Layout o)
        {
            o.Site = site;
            if (!o.HasParentVersion() && Relations(o).Count() > 0)
            {
                throw new KoobooException(string.Format("'{0}' is being used.".Localize(), o.Name));
            }
            base.Remove(site, o);
        }
        #endregion

        #region Localize
        public virtual void Localize(string name, Site targetSite)
        {
            var source = new Models.Layout(targetSite, name).LastVersion();
            Provider.Localize(source, targetSite);
        }
        #endregion

        #region RelationsPages

        public override IEnumerable<RelationModel> Relations(Layout o)
        {
            o = o.AsActual();
            var pageRepository = (IPageProvider)Providers.ProviderFactory.GetProvider<IProvider<Page>>();
            return (pageRepository).ByLayout(o)
                .Select(it => new RelationModel()
                {
                    DisplayName = it.FriendlyName,
                    ObjectUUID = it.FullName,
                    RelationObject = it,
                    RelationType = "Page"
                });
        }
        #endregion

        #region Copy

        public virtual void Copy(Site site, string sourceName, string destName)
        {
            Providers.LayoutProvider.Copy(site, sourceName, destName);
        }

        #endregion
    }
}
