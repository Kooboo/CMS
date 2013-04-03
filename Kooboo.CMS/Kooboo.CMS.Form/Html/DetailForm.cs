using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Form.Html
{
    public class DetailForm : ISchemaForm
    {
        public string Generate(ISchema schema)
        {
            StringBuilder sb = new StringBuilder("");
            sb.Append("@if (Model != null)\r\n{ \r\n");
            sb.AppendFormat("<div>\r\n", schema.Name);
            sb.AppendFormat("\t<h3 class=\"title\" @ViewHelper.Edit(Model,\"{0}\")>@Html.Raw(Model.{0} ?? \"\")</h3>\r\n", schema.TitleColumn == null ? "Title" : schema.TitleColumn.Name);
            sb.Append("\t<div class=\"content\">\r\n");
            foreach (var column in schema.Columns.Where(it => !it.IsSystemField))
            {
                if (schema.TitleColumn != null && string.Compare(column.Name, schema.TitleColumn.Name, true) != 0)
                {
                    sb.AppendFormat("\t\t<div @ViewHelper.Edit(Model,\"{0}\")>@Html.Raw(Model.{0} ?? \"\")</div>\r\n", column.Name);
                }
            }
            sb.Append("\t</div>\r\n</div>");
            sb.Append("\r\n}");
            return sb.ToString();
        }
    }
}
