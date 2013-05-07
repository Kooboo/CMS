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
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Models;
using System.Runtime.Serialization;

namespace Kooboo.CMS.Sites.DataRule
{
    [DataContract(Name = "CategorizableDataRule")]
    [KnownTypeAttribute(typeof(CategorizableDataRule))]
    public class CategorizableDataRule : FolderDataRule
    {
        [DataMember(Order = 13)]
        public string CategoryFolderName { get; set; }
        [DataMember(Order = 15)]
        public IEnumerable<WhereClause> CategoryClauses { get; set; }

        public override Content.Query.IContentQuery<Content.Models.ContentBase> Execute(DataRuleContext dataRuleContext)
        {            
            var contentQuery = base.Execute(dataRuleContext);
            if (contentQuery is MediaContentQuery)
            {
                throw new KoobooException(string.Format("The binary folder '{0}' does not support '{1}'.", FolderName, "CategorizableDataRule"));
            }
            var site = dataRuleContext.PageRequestContext.Site;
            var repositoryName = site.Repository;
            var repository = new Repository(repositoryName);
            var categorizableFolder = (TextFolder)(new TextFolder(repository, CategoryFolderName).AsActual());
            contentQuery = ((TextContentQuery)contentQuery);//.Categorizables(categorizableFolder);
            if (CategoryClauses != null)
            {
                foreach (var clause in CategoryClauses)
                {
                    contentQuery = clause.Parse(contentQuery, dataRuleContext);
                }
            }
            return contentQuery;
        }

        public override DataRuleType DataRuleType
        {
            get
            {
                return DataRule.DataRuleType.Categorizable;
            }
        }
    }
}
