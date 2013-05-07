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


namespace Kooboo.CMS.Form.Html.Controls
{
    public abstract class ControlBase : IControl
    {
        #region IControl Members

        public abstract string Name { get; }

        protected static string EditorTemplate = @"<tr>
            <th>
            <label for=""{0}"">{1}</label>
            </th>
            <td>
            {2}            
            @Html.ValidationMessageForColumn(((ISchema)ViewData[""Schema""])[""{0}""], null)
            {3}
            </td>          
            </tr>";
        public virtual string Render(ISchema schema, IColumn column)
        {
            string html = string.Format(EditorTemplate, column.Name,
                (string.IsNullOrEmpty(column.Label) ? column.Name : column.Label).RazorHtmlEncode(), RenderInput(column),
                FormHelper.Tooltip(column.Tooltip));

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
