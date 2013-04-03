using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Kooboo.CMS.Form.Html.Controls
{
    public abstract class ControlBase : IControl
    {
        #region IControl Members

        public abstract string Name { get; }

        protected static string EditorTemplate = @"<tr>
            <td>
            <label for=""{0}"">{1}</label>{3}
            </td>
            <td>
            {2}
            @Html.ValidationMessageForColumn(((ISchema)ViewData[""Schema""])[""{0}""], null)
            </td>          
            </tr>";
        public virtual string Render(ISchema schema, IColumn column)
        {
            string html = string.Format(EditorTemplate, column.Name,
                (string.IsNullOrEmpty(column.Label) ? column.Name : column.Label).RazorHtmlEncode(), RenderInput(column),
                string.IsNullOrEmpty(column.Tooltip) ? "" : string.Format(@"<a href=""javascript:;"" class=""tooltip-link"" title='{0}'></a>", (column.Tooltip).RazorHtmlEncode()));

            return html;
        }
        protected abstract string RenderInput(IColumn column);

        public virtual bool IsFile
        {
            get { return false; }
        }
        public virtual bool HasDataSource
        {
            get
            {
                return false;
            }
        }
        public virtual string GetValue(object oldValue, string newValue)
        {
            return newValue;
        }
        #endregion
    }
}
