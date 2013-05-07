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

namespace Kooboo.CMS.Content.Query.Translator.String
{
    public class TextContentQueryTranslator : ITranslator<TextContent>
    {
        public TranslatedQuery Translate(Query.IContentQuery<TextContent> contentQuery)
        {
            if (contentQuery is CategoriesQuery)
            {
                var categoriesQuery = (CategoriesQuery)contentQuery;
                var translated = new TranslatedCategoriesQuery(categoriesQuery.Repository, categoriesQuery.CategoryFolder);
                translated.SubQuery = this.Translate(categoriesQuery.InnerQuery);

                StringVisitor visitor = new StringVisitor(translated);
                visitor.Visite(contentQuery.Expression);

                return translated;
            }
            //else if (contentQuery is CategorizablesQuery)
            //{
            //    var categorizablesQuery = (CategorizablesQuery)contentQuery;
            //    var translated = new TranslatedCategorizablesQuery(categorizablesQuery.Repository, categorizablesQuery.CategorizableFolder);
            //    translated.CategoryQuery = this.Translate(categorizablesQuery.CategoryQuery);

            //    StringVisitor visitor = new StringVisitor(translated);
            //    visitor.Visite(contentQuery.Expression);

            //    return translated;
            //}
            else if (contentQuery is ChildrenQuery)
            {
                var childrenQuery = (ChildrenQuery)contentQuery;
                var translated = new TranslatedChildrenQuery(childrenQuery.Repository, childrenQuery.ChildSchema);
                translated.ParentQuery = this.Translate(childrenQuery.ParentQuery);

                StringVisitor visitor = new StringVisitor(translated);
                visitor.Visite(contentQuery.Expression);

                return translated;
            }
            else if (contentQuery is ParentQuery)
            {
                var parentQuery = (ParentQuery)contentQuery;
                var translated = new TranslatedParentQuery(parentQuery.Repository, parentQuery.ParentSchema);
                translated.ChildrenQuery = this.Translate(parentQuery.ChildrenQuery);


                StringVisitor visitor = new StringVisitor(translated);
                visitor.Visite(contentQuery.Expression);

                return translated;
            }
            else if (contentQuery is TextContentQuery)
            {
                var textContentQuery = (TextContentQuery)contentQuery;
                var translated = new TranslatedTextContentQuery(textContentQuery.Repository, textContentQuery.Schema, textContentQuery.Folder);

                StringVisitor visitor = new StringVisitor(translated);
                visitor.Visite(contentQuery.Expression);

                return translated;
            }
            return null;
        }
    }
}
