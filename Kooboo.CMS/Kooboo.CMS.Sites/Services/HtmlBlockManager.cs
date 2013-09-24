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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Kooboo.CMS.Sites.Services
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(HtmlBlockManager))]
    public class HtmlBlockManager : PathResourceManagerBase<HtmlBlock, IHtmlBlockProvider>
    {
        #region .ctor
        public HtmlBlockManager(IHtmlBlockProvider provider) : base(provider) { }
        #endregion

        #region Get
        public override HtmlBlock Get(Site site, string name)
        {
            return new HtmlBlock(site, name).AsActual();
        }
        #endregion

        #region Localize
        public virtual void Localize(string name, Site targetSite)
        {
            var source = new Models.HtmlBlock(targetSite, name).LastVersion();
            Provider.Localize(source, targetSite);
        }
        #endregion

        #region Add
        public override void Add(Site site, HtmlBlock o)
        {
            base.Add(site, o);

            VersionManager.LogVersion(o);
        }
        #endregion

        #region Update
        public override void Update(Site site, HtmlBlock @new, HtmlBlock old)
        {
            base.Update(site, @new, old);

            VersionManager.LogVersion(@new);
        }
        #endregion

        #region Relations
        public override IEnumerable<RelationModel> Relations(HtmlBlock htmlBlock)
        {
            htmlBlock = htmlBlock.AsActual();
            var pageRepository = (IPageProvider)Providers.ProviderFactory.GetProvider<IProvider<Page>>();
            return (pageRepository).ByHtmlBlock(htmlBlock).Select(it => new RelationModel()
            {
                DisplayName = it.FriendlyName,
                ObjectUUID = it.FullName,
                RelationObject = it,
                RelationType = "Page"
            });
        }
        #endregion
    }
}
