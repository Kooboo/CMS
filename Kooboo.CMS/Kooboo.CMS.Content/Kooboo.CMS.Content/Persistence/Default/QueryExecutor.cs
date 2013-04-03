using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Kooboo.CMS.Content.Query.Expressions;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.Linq;
using Kooboo.Extensions;
namespace Kooboo.CMS.Content.Persistence.Default
{
    internal static class ContentQueryExecutor
    {
        public static object Execute(IContentQuery<TextContent> contentQuery)
        {
            QueryExecutorBase queryExecutor = null;
            if (contentQuery is CategoriesQuery)
            {
                queryExecutor = new CategoriesQueryExecutor((CategoriesQuery)contentQuery);
            }
            //else if (contentQuery is CategorizablesQuery)
            //{
            //    queryExecutor = new CategorizablesQueryExecutor((CategorizablesQuery)contentQuery);
            //}
            else if (contentQuery is ParentQuery)
            {
                queryExecutor = new ParentQueryExecutor((ParentQuery)contentQuery);
            }
            else if (contentQuery is ChildrenQuery)
            {
                queryExecutor = new ChildrenQueryExecutor((ChildrenQuery)contentQuery);
            }
            else if (contentQuery is TextContentQuery)
            {
                queryExecutor = new TextContentQueryExecutor((TextContentQuery)contentQuery);
            }
            return queryExecutor.Execute();
        }
    }
    internal abstract class QueryExecutorBase
    {
        public abstract object Execute();

        protected virtual object Execute(IQueryable<TextContent> queryable, IEnumerable<OrderExpression> orders, CallType callType, int? skip, int? take)
        {
            #region Order query
            IOrderedQueryable<TextContent> ordered = null;
            foreach (var orderItem in orders)
            {
                if (!orderItem.Descending)
                {
                    if (ordered == null)
                    {
                        ordered = queryable.OrderBy(orderItem.OrderExprssion);
                    }
                    else
                    {
                        ordered = ordered.ThenBy(orderItem.OrderExprssion);
                    }

                }
                else
                {
                    if (ordered == null)
                    {
                        ordered = queryable.OrderByDescending(orderItem.OrderExprssion);
                    }
                    else
                    {
                        ordered = ordered.ThenByDescending(orderItem.OrderExprssion);
                    }
                }
            }
            if (ordered != null)
            {
                queryable = ordered;
            }

            #endregion


            if (skip.HasValue)
            {
                queryable = queryable.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                queryable = queryable.Take(take.Value);
            }
            switch (callType)
            {
                case Kooboo.CMS.Content.Query.Expressions.CallType.Count:
                    return queryable.Count();
                case Kooboo.CMS.Content.Query.Expressions.CallType.First:
                    return queryable.First();
                case Kooboo.CMS.Content.Query.Expressions.CallType.Last:
                    return queryable.Last();
                case Kooboo.CMS.Content.Query.Expressions.CallType.LastOrDefault:
                    return queryable.LastOrDefault();
                case Kooboo.CMS.Content.Query.Expressions.CallType.FirstOrDefault:
                    return queryable.FirstOrDefault();
                case Kooboo.CMS.Content.Query.Expressions.CallType.Unspecified:
                default:
                    return queryable.ToArray();
            }
        }
    }
    internal class TextContentQueryExecutor : QueryExecutorBase
    {
        public TextContentQuery TextContentQuery { get; private set; }
        public TextContentQueryExecutor(TextContentQuery textContentQuery)
        {
            TextContentQuery = textContentQuery;
        }
        public override object Execute()
        {
            IEnumerable<TextContent> contents = new TextContent[0];

            contents = TextContentQuery.Schema.GetContents();

            if (TextContentQuery.Folder != null)
            {
                contents = contents.Where(it => it.FolderName.EqualsOrNullEmpty(TextContentQuery.Folder.FullName, StringComparison.CurrentCultureIgnoreCase));
            }

            QueryExpressionTranslator translator = new QueryExpressionTranslator();

            var contentQueryable = translator.Translate(TextContentQuery.Expression, contents.AsQueryable());

            foreach (var categoryQuery in translator.CategoryQueries)
            {
                var categories = (IEnumerable<TextContent>)ContentQueryExecutor.Execute(categoryQuery);


                var categoryData = TextContentQuery.Repository.GetCategoryData()
                    .Where(it => categories.Any(c => it.CategoryUUID.EqualsOrNullEmpty(c.UUID, StringComparison.CurrentCultureIgnoreCase)));

                contentQueryable = contentQueryable.Where(it => categoryData.Any(c => it.UUID.EqualsOrNullEmpty(c.ContentUUID, StringComparison.CurrentCultureIgnoreCase)));

            }

            return Execute(contentQueryable, translator.OrderExpressions, translator.CallType, translator.Skip, translator.Take);
        }
    }
    internal class CategoriesQueryExecutor : QueryExecutorBase
    {
        public CategoriesQuery CategoriesQuery { get; private set; }
        public CategoriesQueryExecutor(CategoriesQuery categoriesQuery)
        {
            this.CategoriesQuery = categoriesQuery;
        }

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
    }
    internal class ParentQueryExecutor : QueryExecutorBase
    {
        public ParentQuery ParentQuery { get; private set; }
        public ParentQueryExecutor(ParentQuery parentQuery)
        {
            this.ParentQuery = parentQuery;
        }

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
    }

    internal class ChildrenQueryExecutor : QueryExecutorBase
    {
        public ChildrenQuery ChildrenQuery { get; private set; }
        public ChildrenQueryExecutor(ChildrenQuery childrenQuery)
        {
            this.ChildrenQuery = childrenQuery;
        }

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
    }
}
