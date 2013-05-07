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
    internal class ParentQueryExecutor : QueryExecutorBase
    {
        #region .ctor
        public ParentQuery ParentQuery { get; private set; }
        public ParentQueryExecutor(ParentQuery parentQuery)
        {
            this.ParentQuery = parentQuery;
        } 
        #endregion

        #region Execute
        public override object Execute()
        {
            var children = (IEnumerable<TextContent>)ContentQueryExecutor.Execute(ParentQuery.ChildrenQuery);
            IQueryable<TextContent> contents = new TextContent[0].AsQueryable();
            if (children.Count() > 0)
            {

                contents = ParentQuery.ParentSchema.GetContents().AsQueryable();

            }

            QueryExpressionTranslator translator = new QueryExpressionTranslator();
            contents = translator.Translate(ParentQuery.Expression, contents);

            contents = contents.Where(it => children.Any(c => c.ParentUUID.EqualsOrNullEmpty(it.UUID, StringComparison.CurrentCultureIgnoreCase)));


            return Execute(contents, translator.OrderExpressions, translator.CallType, translator.Skip, translator.Take);
        } 
        #endregion
    }
}
