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

namespace Kooboo.CMS.Content.Persistence.SqlServer.QueryProcessor
{
    public interface IContentQueryTranslator<T>
        where T : ContentBase
    {
        IQueryExecutor<T> Translate(Query.IContentQuery<T> contentQuery);
    }
}
