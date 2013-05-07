#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Sites.View;
using Kooboo.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
namespace Kooboo.CMS.Sites.DataRule
{
    [DataContract(Name = "FolderDataRule")]
    [KnownTypeAttribute(typeof(FolderDataRule))]
    public class FolderDataRule : DataRuleBase
    {
        [DataMember(Order = 13)]
        public string CategoryFolderName { get; set; }

        private WhereClause[] _categoryClauses;
        [DataMember(Order = 15)]
        //public WhereClause[] CategoryClauses
        //{
        //    get
        //    {
        //        return _categoryClauses ?? (_categoryClauses = new WhereClause[0]);
        //    }
        //    set
        //    {
        //        _categoryClauses = value;
        //    }
        //}
        public WhereClause[] CategoryClauses
        {
            get;
            set;
        }

        //[DataMember(Order = 13)]
        //public new string FolderName { get; set; }
        public override Content.Query.IContentQuery<Content.Models.TextContent> Execute(DataRuleContext dataRuleContext)
        {
            var site = dataRuleContext.Site;
            var repository = Sites.Models.ModelExtensions.GetRepository(site);
            if (repository == null)
            {
                throw new SiteRepositoryNotExists();
            }
            var folder = FolderHelper.Parse<TextFolder>(repository, FolderName).AsActual();
            if (folder == null)
            {
                throw new KoobooException(string.Format("The folder does not exists.\"{0}\"".Localize(), FolderName));
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
                contentQuery = contentQuery.Where(WhereClauses.Parse(folder.GetSchema(), dataRuleContext.ValueProvider));
            }
            if (!string.IsNullOrEmpty(CategoryFolderName) && this.CategoryClauses != null && this.CategoryClauses.Length > 0)
            {
                var categoryFolder = FolderHelper.Parse<TextFolder>(repository, CategoryFolderName);
                var categoryQuery = categoryFolder.CreateQuery();
                var expression = CategoryClauses.Parse(categoryFolder.GetSchema(), dataRuleContext.ValueProvider);
                if (expression != null)
                {
                    categoryQuery = categoryQuery.Where(expression);
                    contentQuery = contentQuery.WhereCategory(categoryQuery);
                }
            }


            //query the Published=null content for inline editor..
            if (Page_Context.Current.EnabledInlineEditing(EditingType.Content))
            {
                //contentQuery = contentQuery.Where(
                //    new Content.Query.Expressions.OrElseExpression(
                //        new Content.Query.Expressions.WhereEqualsExpression(null, "Published", true),
                //        new Content.Query.Expressions.WhereEqualsExpression(null, "Published", null)));
            }
            else
                contentQuery = contentQuery.WhereEquals("Published", true); //default query published=true.
            return contentQuery;
        }

        public override DataRuleType DataRuleType
        {
            get { return DataRule.DataRuleType.Folder; }
        }

        public override Schema GetSchema(Repository repository)
        {
            return FolderHelper.Parse<TextFolder>(repository, FolderName).GetSchema();
        }

        public override bool HasAnyParameters()
        {
            if (base.HasAnyParameters())
                return true;
            if (CategoryClauses != null)
            {
                foreach (var item in CategoryClauses)
                {
                    if (ParameterizedFieldValue.IsParameterizedField(item.Value1) || ParameterizedFieldValue.IsParameterizedField(item.Value2))
                    {
                        return true;
                    }
                }
            }
            return false;
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
