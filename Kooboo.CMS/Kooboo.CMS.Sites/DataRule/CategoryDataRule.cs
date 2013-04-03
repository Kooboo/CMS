using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using System.Runtime.Serialization;
using Kooboo.Globalization;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Sites.View;
namespace Kooboo.CMS.Sites.DataRule
{
    [DataContract(Name = "CategoryDataRule")]
    [KnownTypeAttribute(typeof(CategoryDataRule))]
    public class CategoryDataRule : FolderDataRule
    {
        public override Content.Query.IContentQuery<Content.Models.TextContent> Execute(DataRuleContext dataRuleContext)
        {
            var contentQuery = base.Execute(dataRuleContext);
            if (contentQuery is MediaContentQuery)
            {
                throw new KoobooException(string.Format("The binary folder '{0}' does not support '{1}'.", FolderName, "CategoryDataRule"));
            }
            var site = dataRuleContext.Site;
            var repository = Sites.Models.ModelExtensions.GetRepository(site);
            var categoryFolder = (TextFolder)(new TextFolder(repository, CategoryFolderName).AsActual());
            if (categoryFolder == null)
            {
                throw new KoobooException(string.Format("The folder does not exists.\"{0}\"".Localize(), CategoryFolderName));
            }
            contentQuery = ((TextContentQuery)contentQuery).Categories(categoryFolder);
            if (CategoryClauses != null)
            {
                contentQuery = contentQuery.Where(CategoryClauses.Parse(categoryFolder.GetSchema(),dataRuleContext.ValueProvider));
            }

            if (Page_Context.Current.EnabledInlineEditing(EditingType.Content))
            {
                contentQuery = contentQuery.Where(
                    new Content.Query.Expressions.OrElseExpression(
                        new Content.Query.Expressions.WhereEqualsExpression(null, "Published", true),
                        new Content.Query.Expressions.WhereEqualsExpression(null, "Published", null)));
            }
            else
                contentQuery = contentQuery.WhereEquals("Published", true); //default query published=true.

            return contentQuery;
        }

        public override DataRuleType DataRuleType
        {
            get
            {
                return DataRule.DataRuleType.Category;
            }
        }
    }
}
