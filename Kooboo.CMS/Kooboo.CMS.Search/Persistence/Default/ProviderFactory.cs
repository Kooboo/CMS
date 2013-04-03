using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Kooboo.CMS.Content.Persistence;

namespace Kooboo.CMS.Search.Persistence.Default
{
    public class ProviderFactory : IProviderFactory
    {
        static Hashtable providers = new Hashtable();
        static ProviderFactory()
        {
            providers.Add(typeof(ISearchSettingProvider), new SearchSettingProvider());
            providers.Add(typeof(ILastActionProvider), new LastActionProvider());
        }

        #region IProviderFactory Members

        public string Name
        {
            get { return "File"; }
        }

        public T GetProvider<T>()
        {
            foreach (var item in providers.Values)
            {
                if (item is T)
                {
                    return (T)item;
                }
            }
            return default(T);
        }

        #endregion
        public void RegisterProvider<T>(T provider)
        {
            providers[typeof(T)] = provider;
        }
    }
}
