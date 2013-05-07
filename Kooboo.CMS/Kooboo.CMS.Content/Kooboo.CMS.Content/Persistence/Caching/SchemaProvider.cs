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
            inner.Import(repository, zipStream, @override);
            repository.ClearCache();
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
            inner.Initialize(schema);
        }
        #endregion

        #region Create
        public Schema Create(Repository repository, string schemaName, System.IO.Stream templateStream)
        {
            ClearObjectCache(new Schema(repository, schemaName));
            return inner.Create(repository, schemaName, templateStream);
        }
        #endregion

        #region Copy
        public Schema Copy(Repository repository, string sourceName, string destName)
        {
            ClearObjectCache(new Schema(repository, destName));
            return inner.Copy(repository, sourceName, destName);
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
