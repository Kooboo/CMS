using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Persistence.Caching
{
    public class CacheProviderFactory : IProviderFactory
    {
        public static System.Runtime.Caching.CacheItemPolicy DefaultCacheItemPolicy = new System.Runtime.Caching.CacheItemPolicy()
        {
            SlidingExpiration = TimeSpan.Parse("00:30:00")
        };
        protected IProviderFactory innerFactory;
        public CacheProviderFactory(IProviderFactory innerFactory)
        {
            this.innerFactory = innerFactory;
        }
        public virtual string Name
        {
            get { return innerFactory.Name + " (With caching)"; }
        }

        public virtual T GetProvider<T>()
        {
            var innerProvider = innerFactory.GetProvider<T>();

            if (innerProvider is ISchemaProvider)
            {
                return (T)(object)new SchemaProvider((ISchemaProvider)innerProvider);
            }

            if (innerProvider is ITextFolderProvider)
            {
                return (T)(object)new TextFolderProvider((ITextFolderProvider)innerProvider);
            }

            if (innerProvider is IMediaFolderProvider)
            {
                return (T)(object)new MediaFolderProvider((IMediaFolderProvider)innerProvider);
            }

            return innerProvider;
        }


        public void RegisterProvider<T>(T provider)
        {
            innerFactory.RegisterProvider<T>(provider);
        }
    }
}
