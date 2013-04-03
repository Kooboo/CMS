using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Sites.DataRule.CodeSnippet
{
    public class DetailCodeSnippet : IDataRuleCodeSnippet
    {
        public string Generate(Repository repository, Models.DataRuleSetting dataRule)
        {
            if (dataRule.DataRule is DataRuleBase)
            {
                var dataRuleBase = (DataRuleBase)dataRule.DataRule;
                var schema = dataRuleBase.GetSchema(repository).AsActual();


                string html = @"
<h2><%: ViewBag.{0}.{1} ?? """" %></h2>
<ul class=""detail {2}-detail"">   
    {3}
</ul>";
                string columnTp = @"
    <li>
        {0}:
        <p> <%: ViewBag.{1}.{0} ?? """" %> </p>
    </li>
";
                var titleField = schema.GetSummarizeColumn().Name;
                schema = schema.AsActual();
                StringBuilder sb = new StringBuilder();
                foreach (var column in schema.Columns)
                {
                    if (!column.Name.EqualsOrNullEmpty(titleField, StringComparison.CurrentCultureIgnoreCase))
                    {
                        sb.AppendFormat(columnTp, column.Name, dataRule.DataName);
                    }
                }

                return string.Format(html, dataRule.DataName, schema.GetSummarizeColumn().Name, schema.Name, sb.ToString());
            }
            return string.Empty;
        }
    }
}
