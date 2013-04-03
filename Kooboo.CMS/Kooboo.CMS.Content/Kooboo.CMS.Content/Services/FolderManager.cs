using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Persistence;
using System.IO;
using Kooboo.CMS.Content.Models.Paths;
using Kooboo.Extensions;

namespace Kooboo.CMS.Content.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FolderTreeNode<T>
        where T : Folder
    {
        public FolderTreeNode()
        {

        }
        public T Folder { get; set; }

        public IEnumerable<FolderTreeNode<T>> Children { get; internal set; }

    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FolderManager<T> : ManagerBase<T, IFolderProvider<T>>
        where T : Folder
    {

        /// <summary>
        /// Gets the specified repository.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="fullName">The full name.</param>
        /// <returns></returns>
        public override T Get(Repository repository, string fullName)
        {
            return FolderHelper.Parse<T>(repository, fullName).AsActual();
        }

        /// <summary>
        /// Alls the specified repository.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="filterName">Name of the filter.</param>
        /// <param name="fullName">The full name.</param>
        /// <returns></returns>
        public virtual IEnumerable<T> All(Repository repository, string filterName, string fullName)
        {
            if (!string.IsNullOrEmpty(fullName))
            {
                var parent = FolderHelper.Parse<T>(repository, fullName);
                return ChildFolders(parent, filterName);
            }
            var result = All(repository, filterName);
            return result;
        }
        /// <summary>
        /// Alls the specified repository.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="filterName">Name of the filter.</param>
        /// <returns></returns>
        public override IEnumerable<T> All(Models.Repository repository, string filterName)
        {
            var r = GetProvider().All(repository).Select(it => it.AsActual());
            if (!string.IsNullOrEmpty(filterName))
            {
                r = r.Where(it => it.Name.Contains(filterName, StringComparison.OrdinalIgnoreCase) ||
                     (!string.IsNullOrEmpty(it.DisplayName) && it.DisplayName.Contains(filterName, StringComparison.OrdinalIgnoreCase)));
            }

            return r;
        }
        /// <summary>
        /// Childs the folders.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <returns></returns>
        public virtual IEnumerable<T> ChildFolders(T parent)
        {
            return ChildFolders(parent, "");
        }
        /// <summary>
        /// Childs the folders.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="filterName">Name of the filter.</param>
        /// <returns></returns>
        public virtual IEnumerable<T> ChildFolders(T parent, string filterName)
        {
            var r = ((IFolderProvider<T>)this.GetProvider()).ChildFolders(parent).Select(it => it.AsActual());
            if (!string.IsNullOrEmpty(filterName))
            {
                r = r.Where(it => it.Name.Contains(filterName, StringComparison.OrdinalIgnoreCase) ||
                    (!string.IsNullOrEmpty(it.DisplayName) && it.DisplayName.Contains(filterName, StringComparison.OrdinalIgnoreCase)));
            }
            return r;
        }

        /// <summary>
        /// Imports the specified repository.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="fullName">The full name.</param>
        /// <param name="override">if set to <c>true</c> [@override].</param>
        public virtual void Import(Repository repository, Stream stream, string fullName, bool @override)
        {
            var folder = FolderHelper.Parse<T>(repository, fullName);
            GetProvider().Import(repository, folder, stream, @override);
        }

        public virtual void Export(Repository repository, Stream stream, IEnumerable<T> model)
        {
            foreach (var item in model)
            {
                item.Repository = repository;
            }
            GetProvider().Export(repository, model, stream);
        }

        public virtual IEnumerable<FolderTreeNode<T>> FolderTrees(Repository repository)
        {
            return All(repository, "").Select(it => GetFolderTreeNode(it));
        }
        private FolderTreeNode<T> GetFolderTreeNode(T folder)
        {
            FolderTreeNode<T> treeNode = new FolderTreeNode<T>() { Folder = folder };
            treeNode.Children = ChildFolders(folder)
                .Select(it => GetFolderTreeNode(it));
            return treeNode;
        }
    }
}
