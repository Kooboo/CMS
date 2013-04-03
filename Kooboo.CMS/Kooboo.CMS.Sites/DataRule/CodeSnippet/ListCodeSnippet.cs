using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Sites.DataRule.CodeSnippet
{
    public class ListCodeSnippet : IDataRuleCodeSnippet
    {
        public string Generate(Repository repository, Models.DataRuleSetting dataRule)
        {
            if (dataRule.DataRule is DataRuleBase)
            {
                var dataRuleBase = (DataRuleBase)dataRule.DataRule;
                var schema = dataRuleBase.GetSchema(repository).AsActual();


                string html = @"
<ul>
    <%foreach (var item in ViewBag.{0})
      {{%>
    <li>
        <%: Html.FrontHtml().PageLink(item.{1}, ""{2}/detail"", new {{ UserKey = item.UserKey}}) %>
    </li>
    <%}}%>
</ul>";

                var snippet = string.Format(html, dataRule.DataName, schema.GetSummarizeColumn().Name, schema.Name);
                if (dataRule.DataRule.EnablePaging())
                {
                    snippet += Environment.NewLine + string.Format("<%: Html.FrontHtml().Pager(ViewBag.{0}) %>", dataRule.DataName);
                }
                return snippet;
            }
            return string.Empty;
        }
    }
}
