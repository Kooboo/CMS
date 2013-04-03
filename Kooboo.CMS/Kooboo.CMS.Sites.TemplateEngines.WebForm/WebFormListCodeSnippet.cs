using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.View.CodeSnippet;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Sites.DataRule;

namespace Kooboo.CMS.Sites.TemplateEngines.WebForm
{
    public class WebFormListCodeSnippet : IDataRuleCodeSnippet
    {
        public string Generate(Repository repository, Models.DataRuleSetting dataRule, bool inlineEdit)
        {
            if (dataRule.DataRule is DataRuleBase)
            {
                var dataRuleBase = (DataRuleBase)dataRule.DataRule;
                var schema = dataRuleBase.GetSchema(repository).AsActual();


                string html = @"
<ul class=""list"">
    <% foreach(dynamic item in ViewBag.{0})
      {{%>
    <li{2}>
        {1}
    </li>
    <%}}%>
</ul>";
                var titleField = schema.GetSummarizeColumn();
                var editItem = "";
                var linkHtml = string.Format("<a class='title' href='<%:@Url.FrontUrl().PageUrl(\"{1}/detail\",new {{ UserKey = item.UserKey}})%>'><%:item.{0}%></a>", titleField.Name, schema.Name);
                if (inlineEdit)
                {
                    editItem = "  <%:ViewHelper.Edit(item)%>";
                    linkHtml = string.Format("<a class='title' href='<%:@Url.FrontUrl().PageUrl(\"{1}/detail\",new {{ UserKey = item.UserKey}})%>' <%:ViewHelper.Edit(item,\"{0}\")%>><%:item.{0}%></a>", titleField.Name, schema.Name);
                }

                var snippet = string.Format(html, dataRule.DataName, linkHtml, editItem);
                if (dataRule.DataRule.EnablePaging())
                {
                    snippet += Environment.NewLine + string.Format(@"
<div class=""pager"">
<%: Html.FrontHtml().Pager(ViewBag.{0}) %>
</div>", dataRule.DataName);
                }
                return snippet;
            }
            return string.Empty;
        }
    }
}
