#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Persistence.Default.ContentQuery
{
    internal class ChildrenQueryExecutor : QueryExecutorBase
    {
        #region .ctor
        public ChildrenQuery ChildrenQuery { get; private set; }
        public ChildrenQueryExecutor(ChildrenQuery childrenQuery)
        {
            this.ChildrenQuery = childrenQuery;
        } 
        #endregion

        #region Execute
        public override object Execute()
        {
            var parent = (IEnumerable<TextContent>)ContentQueryExecutor.Execute(ChildrenQuery.ParentQuery);
            IQueryable<TextContent> contents = new TextContent[0].AsQueryable();
            if (parent.Count() > 0)
            {

                contents = ChildrenQuery.ChildSchema.GetContents().AsQueryable();

            }

            QueryExpressionTranslator translator = new QueryExpressionTranslator();
            contents = translator.Translate(ChildrenQuery.Expression, contents);

            contents = contents.Where(it => parent.Any(c => it.ParentUUID.EqualsOrNullEmpty(c.UUID, StringComparison.CurrentCultureIgnoreCase)));


            return Execute(contents, translator.OrderExpressions, translator.CallType, translator.Skip, translator.Take);
        } 
        #endregion
    }
}
