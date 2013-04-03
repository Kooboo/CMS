using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.Extensions;
using Kooboo.Globalization;
using Kooboo.CMS.Sites.Versioning;
using System.IO;
namespace Kooboo.CMS.Sites.Services
{
    public class ViewManager : PathResourceManagerBase<Kooboo.CMS.Sites.Models.View, IViewProvider>
    {
        public IVersionLogger<Models.View> VersiongLogger
        {
            get
            {
                return VersionManager.ResolveVersionLogger<Models.View>();
            }
        }
        public virtual Namespace<Models.View> GetNamespace(Site site, params string[] exculdes)
        {
            var views = All(site, "").Where(it => !exculdes.Any(ex => ex.EqualsOrNullEmpty(it.Name, StringComparison.OrdinalIgnoreCase)));

            Dictionary<string, Namespace<Models.View>> namespaces = new Dictionary<string, Namespace<Models.View>>(StringComparer.CurrentCultureIgnoreCase);
            Namespace<Models.View> root = new Namespace<Models.View>();
            foreach (var item in views)
            {
                string parentName = "";
                Namespace<Models.View> last = null;
                var names = item.Name.Split(".".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (var name in names.Take(names.Length - 1))
                {
                    string fullName = "";
                    if (string.IsNullOrEmpty(parentName))
                    {
                        fullName = name;
                    }
                    else
                    {
                        fullName = parentName + "." + name;
                    }
                    if (!namespaces.ContainsKey(fullName))
                    {
                        last = new Namespace<Models.View>()
                        {
                            Name = name,
                            FullName = fullName
                        };
                        namespaces[fullName] = last;
                        if (!string.IsNullOrEmpty(parentName))
                        {
                            namespaces[parentName].AddNamespace(last);
                        }
                        else
                        {
                            root.AddNamespace(last);
                        }
                    }
                    else
                    {
                        last = namespaces[fullName];
                    }
                    parentName = fullName;
                }
                if (last == null)
                {
                    root.AddEntry(item.Name, item);
                }
                else
                {
                    last.AddEntry(item.Name, item);
                }
            }
            return root;
        }

        public virtual IEnumerable<Kooboo.CMS.Sites.Models.View> ByNamespace(Site site, string ns, string filter)
        {
            if (string.IsNullOrEmpty(ns))
            {
                return All(site, filter).Where(it => !it.Name.Contains(".", StringComparison.CurrentCultureIgnoreCase));
            }
            else
                return All(site, filter).Where(it => it.Name.StartsWith(ns + ".", StringComparison.CurrentCultureIgnoreCase)
                    && it.Name.Split(".".ToArray(), StringSplitOptions.RemoveEmptyEntries).Length == ns.Split(".".ToArray(), StringSplitOptions.RemoveEmptyEntries).Length + 1);
        }
        public override Models.View Get(Models.Site site, string name)
        {
            return Provider.Get(new Models.View(site, name));
        }

        public override void Remove(Site site, Models.View o)
        {
            o.Site = site;
            if (!o.HasParentVersion() && RelationsPages(o).Where(i => i.Site == site).Count() > 0)
            {
                throw new KoobooException(string.Format("'{0}' is being used".Localize(), o.Name));
            }
            base.Remove(site, o);
        }

        private bool HasSameName(Site site, string name)
        {
            if (site.Parent != null)
            {
                var query = Get(site.Parent, name);
                if (query != null)
                { throw new ItemAlreadyExistsException(); }
                else
                {
                    return HasSameName(site.Parent, name);
                }
            }
            return false;
        }
        public override void Update(Site site, Models.View @new, Models.View old)
        {
            base.Update(site, @new, old);

            VersionManager.LogVersion(@new);
        }

        public override void Add(Site site, Models.View o)
        {
            HasSameName(site, o.Name);

            base.Add(site, o);

            VersionManager.LogVersion(o);

        }

        public virtual void Localize(string name, Site targetSite)
        {
            var source = new Models.View(targetSite, name).LastVersion();
            ((IViewProvider)Provider).Localize(source, targetSite);
        }

        #region Relations
        public virtual IEnumerable<Page> RelationsPages(Models.View view)
        {
            if (view.Exists())
            {
                view = view.AsActual();
                var pageRepository = (IPageProvider)Providers.ProviderFactory.GetRepository<IProvider<Page>>();
                return pageRepository.ByView(view);
            }
            return new Page[0];

        }
        #endregion

        #region Copy

        public virtual void Copy(Site site, string sourceName, string destName)
        {
            ((IViewProvider)Provider).Copy(site, sourceName, destName);
        }

        #endregion
    }
}
