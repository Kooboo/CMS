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
using Kooboo.CMS.Content.Persistence;

namespace Kooboo.CMS.Content.Query
{
    public static class RepositoryQueryExtensions
    {
        public static IEnumerable<IDictionary<string, object>> ExecuteQuery(this Repository repository, string queryText, System.Data.CommandType commandType = System.Data.CommandType.Text, params KeyValuePair<string, object>[] parameters)
        {
            return Persistence.Providers.DefaultProviderFactory.GetProvider<ITextContentProvider>().ExecuteQuery(repository, queryText, commandType, parameters);
        }

        public static object ExecuteScalar(this Repository repository, string queryText, System.Data.CommandType commandType = System.Data.CommandType.Text, params  KeyValuePair<string, object>[] parameters)
        {
            return Persistence.Providers.DefaultProviderFactory.GetProvider<ITextContentProvider>().ExecuteScalar(repository, queryText, commandType, parameters);
        }


        public static void ExecuteNonQuery(this Repository repository, string queryText, System.Data.CommandType commandType = System.Data.CommandType.Text, params  KeyValuePair<string, object>[] parameters)
        {
            Persistence.Providers.DefaultProviderFactory.GetProvider<ITextContentProvider>().ExecuteNonQuery(repository, queryText, commandType, parameters);
        }
    }
}
