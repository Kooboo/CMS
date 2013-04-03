using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Persistence;

namespace Kooboo.CMS.Content.Query
{
    public static class RepositoryQueryExtensions
    {
        public static IEnumerable<IDictionary<string, object>> ExecuteQuery(this Repository repository, string queryText, params KeyValuePair<string, object>[] parameters)
        {
            return Persistence.Providers.DefaultProviderFactory.GetProvider<ITextContentProvider>().ExecuteQuery(repository, queryText, parameters);
        }

        public static object ExecuteScalar(this Repository repository, string queryText, params  KeyValuePair<string, object>[] parameters)
        {
            return Persistence.Providers.DefaultProviderFactory.GetProvider<ITextContentProvider>().ExecuteScalar(repository, queryText, parameters);
        }


        public static void ExecuteNonQuery(this Repository repository, string queryText, params  KeyValuePair<string, object>[] parameters)
        {
            Persistence.Providers.DefaultProviderFactory.GetProvider<ITextContentProvider>().ExecuteNonQuery(repository, queryText, parameters);
        }
    }
}
