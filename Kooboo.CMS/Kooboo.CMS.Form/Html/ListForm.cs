using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Form.Html
{
    public class ListForm : ISchemaForm
    {
        public string Generate(ISchema schema)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(@"<h3>{0}</h3>
<ul class=""list"">
    @foreach (var item in Model)
      {{
        <li>", schema.Name);
            sb.AppendFormat(@"@Html.FrontHtml().PageLink(item[""{0}""], ""{1}/detail"", new {{ userKey = item[""UserKey""] }}, new {{ @class=""title"" }})", schema.TitleColumn == null ? "Title" : schema.TitleColumn.Name, schema.Name);
            sb.Append(@"
        </li>
      }
</ul>");
            return sb.ToString();
        }
    }
}
