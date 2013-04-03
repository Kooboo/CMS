using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.CMS.Sites.Models;
using Kooboo.Globalization;
using Kooboo.CMS.Sites.Versioning;

namespace Kooboo.CMS.Sites.Services
{
    public class LayoutManager : PathResourceManagerBase<Layout, ILayoutProvider>
    {
        public IVersionLogger<Models.Layout> VersiongLogger
        {
            get
            {
                return VersionManager.ResolveVersionLogger<Models.Layout>();
            }
        }
        public override Layout Get(Site site, string name)
        {
            return Provider.Get(new Layout(site, name));
        }

        public override void Remove(Site site, Layout o)
        {
            o.Site = site;
            if (!o.HasParentVersion() && RelationsPages(o).Where(it => it.Site == site).Count() > 0)
            {
                throw new KoobooException(string.Format("'{0}' is being used.".Localize(), o.Name));
            }
            base.Remove(site, o);
        }

        public virtual void Localize(string name, Site targetSite)
        {
            var source = new Models.Layout(targetSite, name).LastVersion();
            Provider.Localize(source, targetSite);
        }

        public virtual IEnumerable<Page> RelationsPages(Layout layout)
        {
            layout = layout.AsActual();
            var pageRepository = (IPageProvider)Providers.ProviderFactory.GetRepository<IProvider<Page>>();
            return (pageRepository).ByLayout(layout);
        }

        public override void Add(Site site, Layout o)
        {
            base.Add(site, o);
            VersionManager.LogVersion(o);
        }
        public override void Update(Site site, Layout @new, Layout old)
        {
            base.Update(site, @new, old);
            VersionManager.LogVersion(@new);
        }

        #region Copy

        public virtual void Copy(Site site, string sourceName, string destName)
        {
            Providers.LayoutProvider.Copy(site, sourceName, destName);
        }

        #endregion
    }
}
