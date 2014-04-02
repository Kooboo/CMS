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
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Caching;
using Kooboo.CMS.Caching;
namespace Kooboo.CMS.Content.Persistence.Caching
{
    public class MediaFolderProvider : CacheProviderBase<MediaFolder>, IMediaFolderProvider
    {
        #region .ctor
        private IMediaFolderProvider inner;
        public MediaFolderProvider(IMediaFolderProvider innerProvider)
            : base(innerProvider)
        {
            inner = innerProvider;
        } 
        #endregion

        #region ChildFolders
        public IQueryable<MediaFolder> ChildFolders(MediaFolder parent)
        {
            return parent.Repository.ObjectCache().GetCache<MediaFolder[]>("MediaFolderProvider.ChildFolders:Parent:" + parent.FullName.ToLower(), () =>
            {
                return inner.ChildFolders(parent).ToArray();
            }).AsQueryable();
        } 
        #endregion

        #region All
        public IEnumerable<MediaFolder> All(Repository repository)
        {
            return repository.ObjectCache().GetCache<MediaFolder[]>("MediaFolderProvider.All", () =>
            {
                return inner.All(repository).ToArray();
            }).AsQueryable();
        } 
        #endregion

        #region Export
        public void Export(Repository repository, IEnumerable<MediaFolder> models, System.IO.Stream outputStream)
        {
            inner.Export(repository, models, outputStream);
        } 
        #endregion

        #region Import
        public void Import(Repository repository, MediaFolder folder, System.IO.Stream zipStream, bool @override)
        {
            try
            {
                inner.Import(repository, folder, zipStream, @override);
            }
            finally
            {
                repository.ClearCache();
            }            
        } 
        #endregion

        #region GetCacheKey
        protected override string GetCacheKey(MediaFolder o)
        {
            return "MediaFolder:" + o.FullName.ToLower();
        } 
        #endregion

        #region All
        public IEnumerable<MediaFolder> All()
        {
            return inner.All();
        } 
        #endregion

        #region Rename
        public void Rename(MediaFolder @new, MediaFolder old)
        {
            try
            {
                inner.Rename(@new, old);
            }
            finally
            {
                @new.Repository.ClearCache();
            }
        } 
        #endregion

        #region Export
        public void Export(Repository repository, string baseFolder, string[] folders, string[] docs, System.IO.Stream outputStream)
        {
            inner.Export(repository, baseFolder, folders, docs, outputStream);
        } 
        #endregion
    }
}
