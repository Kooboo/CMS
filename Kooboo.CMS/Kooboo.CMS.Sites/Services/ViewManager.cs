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

using Kooboo.Common.Globalization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace Kooboo.CMS.Sites.Services
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(ViewManager))]
    public class ViewManager : PathResourceManagerBase<Kooboo.CMS.Sites.Models.View, IViewProvider>
    {
        #region .ctor
        public ViewManager(IViewProvider provider)
            : base(provider)
        {
        }
        #endregion

        #region GetNamespace
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
        #endregion

        #region Add
        public override void Add(Site site, Models.View o)
        {
            HasSameName(site, o.Name);

            base.Add(site, o);

            VersionManager.LogVersion(o);

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
        #endregion

        #region Get
        public override Models.View Get(Models.Site site, string name)
        {
            return Provider.Get(new Models.View(site, name));
        }
        #endregion

        #region Update
        public override void Update(Site site, Models.View @new, Models.View old)
        {
            base.Update(site, @new, old);

            VersionManager.LogVersion(@new);
        }
        #endregion

        #region Remove
        public override void Remove(Site site, Models.View o)
        {
            o.Site = site;
            if (!o.HasParentVersion() && Relations(o).Count() > 0)
            {
                throw new Exception(string.Format("'{0}' is being used".Localize(), o.Name));
            }
            base.Remove(site, o);
        }
        #endregion

        #region Localize
        public virtual void Localize(string name, Site targetSite, string userName = null)
        {
            var target = new Models.View(targetSite, name);
            var source = target.LastVersion();
            if (target.Site != source.Site)
            {
                ((IViewProvider)Provider).Localize(source, targetSite);
                target = target.AsActual();
                if (target != null)
                {
                    target.UserName = userName;
                    Update(targetSite, target, target);
                }
            }

        }
        #endregion

        #region Relations
        public override IEnumerable<RelationModel> Relations(Models.View view)
        {
            view = view.AsActual();
            var pageRepository = (IPageProvider)Providers.ProviderFactory.GetProvider<IProvider<Page>>();
            return pageRepository.ByView(view).Select(it => new RelationModel()
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
            ((IViewProvider)Provider).Copy(site, sourceName, destName);
        }

        #endregion
    }
}
