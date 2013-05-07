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
    internal class CategoriesQueryExecutor : QueryExecutorBase
    {
        #region .ctor
        public CategoriesQuery CategoriesQuery { get; private set; }
        public CategoriesQueryExecutor(CategoriesQuery categoriesQuery)
        {
            this.CategoriesQuery = categoriesQuery;
        } 
        #endregion

        #region Execute
        public override object Execute()
        {
            var contents = (IEnumerable<TextContent>)ContentQueryExecutor.Execute(CategoriesQuery.InnerQuery);
            IQueryable<TextContent> categories = new TextContent[0].AsQueryable();
            if (contents.Count() > 0)
            {
                categories = CategoriesQuery.CategoryFolder.GetSchema().GetContents().AsQueryable();
            }

            QueryExpressionTranslator translator = new QueryExpressionTranslator();


            categories = translator.Translate(CategoriesQuery.Expression, categories);


            var categoryData = CategoriesQuery.Repository.GetCategoryData()
                .Where(it => it.CategoryFolder.EqualsOrNullEmpty(CategoriesQuery.CategoryFolder.FullName, StringComparison.CurrentCultureIgnoreCase))
                .Where(it => contents.Any(c => it.ContentUUID.EqualsOrNullEmpty(c.UUID, StringComparison.CurrentCultureIgnoreCase)))
                .ToArray();

            categories = categories.Where(it => categoryData.Any(c => it.UUID.EqualsOrNullEmpty(c.CategoryUUID, StringComparison.CurrentCultureIgnoreCase)));

            var result = Execute(categories, translator.OrderExpressions, translator.CallType, translator.Skip, translator.Take);

            return result;
        } 
        #endregion
    }
}
