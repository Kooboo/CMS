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
using Kooboo.CMS.Common.Persistence.Non_Relational;
namespace Kooboo.CMS.Content.Persistence.Caching
{
    public class RepositoryProvider : IRepositoryProvider
    {
        #region .ctor
        private IRepositoryProvider inner;
        public RepositoryProvider(IRepositoryProvider innerProvider)
        {
            inner = innerProvider;
        }
        #endregion

        #region All
        public IEnumerable<Models.Repository> All()
        {
            return inner.All();
        }
        #endregion

        #region Get
        public Models.Repository Get(Models.Repository dummy)
        {
            var cacheKey = GetCacheKey(dummy);
            var o = (Repository)dummy.ObjectCache().Get(cacheKey);
            if (o == null)
            {
                o = inner.Get(dummy);
                if (o == null)
                {
                    return o;
                }
                dummy.ObjectCache().Add(cacheKey, o, Kooboo.CMS.Caching.ObjectCacheExtensions.DefaultCacheItemPolicy);
            }
            return o;
        }
        #endregion

        #region GetCacheKey
        private string GetCacheKey(Repository repository)
        {
            return "Repository:" + repository.Name.ToLower();
        }
        #endregion

        #region Add
        public void Add(Models.Repository item)
        {
            inner.Add(item);
        }
        #endregion
        
        #region Update
        public void Update(Models.Repository @new, Models.Repository old)
        {
            inner.Update(@new, old);
            var cacheKey = GetCacheKey(@new);
            @new.ObjectCache().Remove(cacheKey);
        }
        #endregion

        #region Remove
        public void Remove(Models.Repository item)
        {
            inner.Remove(item);
            @item.ClearCache();
        }
        #endregion

        #region Wrapper methods

        #region Create
        public Repository Create(string repositoryName, System.IO.Stream templateStream)
        {
            return inner.Create(repositoryName, templateStream);
        }
        #endregion

        #region Initialize
        public void Initialize(Repository repository)
        {
            inner.Initialize(repository);
        }
        #endregion

        #region Export
        public void Export(Repository repository, System.IO.Stream outputStream)
        {
            inner.Export(repository, outputStream);
        }
        #endregion

        public void Offline(Repository repository)
        {
            inner.Offline(repository);
        }

        public void Online(Repository repository)
        {
            inner.Online(repository);
        }

        public bool IsOnline(Repository repository)
        {
            return inner.IsOnline(repository);
        }


        public Repository Copy(Repository sourceRepository, string destRepositoryName)
        {
            return inner.Copy(sourceRepository, destRepositoryName);
        }


        public bool TestDbConnection()
        {
            return inner.TestDbConnection();
        }

        IEnumerable<Repository> IProvider<Repository>.All()
        {
            return inner.All();
        }
        #endregion
    }
}
