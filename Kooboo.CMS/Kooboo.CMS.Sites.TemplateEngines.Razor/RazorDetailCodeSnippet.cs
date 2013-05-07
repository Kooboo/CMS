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
using Kooboo.CMS.Sites.DataRule;
using Kooboo.CMS.Sites.View.CodeSnippet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Kooboo.CMS.Sites.TemplateEngines.Razor
{
    public class RazorDetailCodeSnippet : IDataRuleCodeSnippet
    {
        public string Generate(Repository repository, Models.DataRuleSetting dataRule, bool inlineEdit)
        {
            if (dataRule.DataRule is DataRuleBase)
            {
                var dataRuleBase = (DataRuleBase)dataRule.DataRule;
                var schema = dataRuleBase.GetSchema(repository).AsActual();


                string html = @"
<div>
    <h3 class=""title""{3}>@Html.Raw(ViewBag.{0}.{1} ?? """")</h3>
    <div class=""content"">   
        {2}
    </div>
</div>";
                string columnTp = @"
        <div{2}>
            @Html.Raw(ViewBag.{1}.{0} ?? """")
        </div>
";
                var titleField = schema.GetSummarizeColumn().Name;
                var editField = " @ViewHelper.Edit(ViewBag.{0},\"{1}\")";
                schema = schema.AsActual();
                StringBuilder sb = new StringBuilder();
                foreach (var column in schema.Columns)
                {
                    if (!column.Name.EqualsOrNullEmpty(titleField, StringComparison.CurrentCultureIgnoreCase))
                    {
                        sb.AppendFormat(columnTp, column.Name, dataRule.DataName, inlineEdit ? string.Format(editField, dataRule.DataName, column.Name) : "");
                    }
                }

                return string.Format(html, dataRule.DataName, schema.GetSummarizeColumn().Name, sb.ToString()
                    , inlineEdit ? string.Format(editField, dataRule.DataName, titleField) : "");
            }
            return string.Empty;
        }
    }
}
