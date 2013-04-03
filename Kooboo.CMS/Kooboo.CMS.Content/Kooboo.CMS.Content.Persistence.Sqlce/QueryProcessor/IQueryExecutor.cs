using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.Persistence.Sqlce.QueryProcessor
{
    public interface IQueryExecutor<T>
        where T : ContentBase
    {
        object Execute();
        string BuildQuerySQL(SQLCeVisitor<T> visitor, out IEnumerable<Parameter> parameters);
    }
}
