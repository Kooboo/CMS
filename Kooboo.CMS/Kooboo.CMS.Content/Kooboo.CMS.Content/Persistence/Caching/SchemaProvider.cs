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
namespace Kooboo.CMS.Content.Persistence.Caching
{
    public class SchemaProvider : CacheProviderBase<Schema>, ISchemaProvider
    {
        #region .ctor
        private ISchemaProvider inner;
        public SchemaProvider(ISchemaProvider innerProvider)
            : base(innerProvider)
        {
            inner = innerProvider;
        }
        #endregion

        #region All
        public IEnumerable<Schema> All(Repository repository)
        {
            return inner.All(repository);
        }
        #endregion

        #region Export
        public void Export(Repository repository, IEnumerable<Schema> models, System.IO.Stream outputStream)
        {
            inner.Export(repository, models, outputStream);
        }

        #endregion

        #region Import
        public void Import(Repository repository, System.IO.Stream zipStream, bool @override)
        {
            try
            {
                inner.Import(repository, zipStream, @override);
            }
            finally
            {
                repository.ClearCache();
            }
        }
        #endregion

        #region GetCacheKey
        protected override string GetCacheKey(Schema o)
        {
            return "Schema:" + o.Name.ToLower();
        }
        #endregion

        #region Initialize
        public void Initialize(Schema schema)
        {
            try
            {
                inner.Initialize(schema);
            }
            finally
            {
                schema.Repository.ClearCache();
            }
        }
        #endregion

        #region Create
        public Schema Create(Repository repository, string schemaName, System.IO.Stream templateStream)
        {
            try
            {
                return inner.Create(repository, schemaName, templateStream);
            }
            finally
            {
                ClearObjectCache(new Schema(repository, schemaName));
            }
        }
        #endregion

        #region Copy
        public Schema Copy(Repository repository, string sourceName, string destName)
        {
            try
            {
                return inner.Copy(repository, sourceName, destName);
            }
            finally
            {
                ClearObjectCache(new Schema(repository, destName));
            }
        }
        #endregion

        #region All
        public IEnumerable<Schema> All()
        {
            return inner.All();
        }
        #endregion
    }
}
