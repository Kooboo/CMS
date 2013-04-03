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
        private ISchemaProvider inner;
        public SchemaProvider(ISchemaProvider innerProvider)
            : base(innerProvider)
        {
            inner = innerProvider;
        }
        public IQueryable<Schema> All(Repository repository)
        {
            return inner.All(repository);
        }

        public void Export(Repository repository, IEnumerable<Schema> models, System.IO.Stream outputStream)
        {
            inner.Export(repository, models, outputStream);
        }

        public void Import(Repository repository, System.IO.Stream zipStream, bool @override)
        {
            inner.Import(repository, zipStream, @override);
            repository.ClearCache();
        }

        protected override string GetCacheKey(Schema o)
        {
            return "Schema:" + o.Name.ToLower();
        }

        public void Initialize(Schema schema)
        {
            inner.Initialize(schema);
        }


        public Schema Create(Repository repository, string schemaName, System.IO.Stream templateStream)
        {
            ClearObjectCache(new Schema(repository, schemaName));
            return inner.Create(repository, schemaName, templateStream);
        }


        public Schema Copy(Repository repository, string sourceName, string destName)
        {
            ClearObjectCache(new Schema(repository, destName));
            return inner.Copy(repository, sourceName, destName);
        }
    }
}
