#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Collections;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.Common.Globalization;
using Kooboo.CMS.Common.Persistence.Non_Relational;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Kooboo.Common.TokenTemplate;



namespace Kooboo.CMS.Sites.DataSource.ContentDataSource
{
    [DataContract(Name = "FolderDataSource")]
    [KnownTypeAttribute(typeof(FolderDataSource))]
    public class FolderDataSource : DataSourceBase
    {
        [DataMember]
        public string CategoryFolderName { get; set; }

        [DataMember]
        public WhereClause[] CategoryClauses
        {
            get;
            set;
        }

        //[DataMember(Order = 13)]
        //public new string FolderName { get; set; }
        public override IContentQuery<TextContent> GetContentQuery(DataSourceContext dataSourceContext)
        {
            var site = dataSourceContext.Site;
            var repository = Sites.Models.ModelExtensions.GetRepository(site);
            if (repository == null)
            {
                throw new SiteRepositoryNotExists();
            }
            var folder = new TextFolder(repository, FolderName).AsActual();
            if (folder == null)
            {
                throw new ArgumentNullException("FolderName", string.Format("The folder does not exists.\"{0}\"".Localize(), FolderName));
            }
            Content.Query.IContentQuery<Content.Models.TextContent> contentQuery = null;
            if (folder is TextFolder)
            {
                var textFolder = (TextFolder)folder;
                contentQuery = textFolder.CreateQuery();
            }
            //else
            //{
            //    var binaryFolder = (MediaFolder)folder;
            //    contentQuery = binaryFolder.CreateQuery();
            //}
            if (WhereClauses != null)
            {
                contentQuery = contentQuery.Where(WhereClauses.Parse(folder.GetSchema(), dataSourceContext.ValueProvider));
            }
            if (!string.IsNullOrEmpty(CategoryFolderName) && this.CategoryClauses != null && this.CategoryClauses.Length > 0)
            {
                var categoryFolder = FolderHelper.Parse<TextFolder>(repository, CategoryFolderName);
                var categoryQuery = categoryFolder.CreateQuery();
                var expression = CategoryClauses.Parse(categoryFolder.GetSchema(), dataSourceContext.ValueProvider);
                if (expression != null)
                {
                    categoryQuery = categoryQuery.Where(expression);
                    contentQuery = contentQuery.WhereCategory(categoryQuery);
                }
            }


            //query the Published=null content for inline editor..
            //if (Page_Context.Current.EnabledInlineEditing(EditingType.Content))
            //{
            //    //contentQuery = contentQuery.Where(
            //    //    new Content.Query.Expressions.OrElseExpression(
            //    //        new Content.Query.Expressions.WhereEqualsExpression(null, "Published", true),
            //    //        new Content.Query.Expressions.WhereEqualsExpression(null, "Published", null)));
            //}
            //else
            contentQuery = contentQuery.WhereEquals("Published", true); //default query published=true.
            return contentQuery;
        }

        public override Schema GetSchema(Repository repository)
        {
            return FolderHelper.Parse<TextFolder>(repository, FolderName).GetSchema();
        }

        public override IEnumerable<string> GetParameters()
        {
            var parser = new TemplateParser();
            var parameters = base.GetParameters().ToList();
            if (CategoryClauses != null)
            {
                foreach (var item in CategoryClauses)
                {
                    if (!string.IsNullOrEmpty(item.Value1))
                    {
                        var value1Parameters = parser.GetTokens(item.Value1);
                        parameters.AddRange(value1Parameters, StringComparer.OrdinalIgnoreCase);
                    }
                    if (!string.IsNullOrEmpty(item.Value2))
                    {
                        var value2Paramters = parser.GetTokens(item.Value2);
                        parameters.AddRange(value2Paramters, StringComparer.OrdinalIgnoreCase);
                    }
                }
            }

            return parameters;
        }

        public override bool IsValid(Kooboo.CMS.Content.Models.Repository repository)
        {
            var valid = base.IsValid(repository);
            if (valid && !string.IsNullOrEmpty(CategoryFolderName))
            {
                var folder = FolderHelper.Parse<TextFolder>(repository, CategoryFolderName);
                return folder.AsActual() != null;
            }
            return valid;
        }
    }
}
