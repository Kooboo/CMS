using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using System.Runtime.Serialization;

namespace Kooboo.CMS.Sites.DataRule
{
    [DataContract(Name = "SchemaDataRule")]
    [KnownTypeAttribute(typeof(SchemaDataRule))]
    public class SchemaDataRule : DataRuleBase
    {
        [DataMember(Order = 13)]
        public string SchemaName { get; set; }

        public override Content.Query.IContentQuery<Content.Models.TextContent> Execute(DataRuleContext dataRuleContext)
        {
            var site = dataRuleContext.Site;
            var repositoryName = site.Repository;
            if (string.IsNullOrEmpty(repositoryName))
            {
                throw new KoobooException("The repository for site is null.");
            }
            var repository = new Repository(repositoryName);
            var schema = new Schema(repository, SchemaName);
            var contentQuery = (IContentQuery<Content.Models.TextContent>)schema.CreateQuery();

            contentQuery.Where(WhereClauses.Parse(schema,dataRuleContext.ValueProvider));

            return contentQuery;
        }
        public override DataRuleType DataRuleType
        {
            get { return DataRuleType.Schema; }
        }

        public override Schema GetSchema(Repository repository)
        {
            return new Schema(repository, SchemaName);
        }
    }
}
