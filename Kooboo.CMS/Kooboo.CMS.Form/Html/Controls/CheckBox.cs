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
    public class CheckBox : ControlBase
    {
        protected static string CheckBoxTemplate = @"<tr>
            <th>
           
            </th>
            <td>
            {2}
            <label class=""inline"" for=""{0}"">{1}</label>{3}
            @Html.ValidationMessageForColumn(((ISchema)ViewData[""Schema""])[""{0}""], null)
            </td>          
            </tr>";
        public override string Name
        {
            get { return "CheckBox"; }
        }
        public override string DataType
        {
            get
            {
                return "Bool";
            }
        }
        protected override string RenderInput(IColumn column)
        {
            return string.Format(@"<input id='{0}' name=""{0}"" type=""checkbox"" @(Convert.ToBoolean(Model.{0})?""checked"":"""") value=""true""/>
                                    <input type=""hidden"" value=""false"" name=""{0}""/>", column.Name);
        }

        public override string Render(ISchema schema, IColumn column)
        {
            string html = string.Format(CheckBoxTemplate, column.Name,
                 (string.IsNullOrEmpty(column.Label) ? column.Name : column.Label).RazorHtmlEncode(), RenderInput(column),
                 FormHelper.Tooltip(column.Tooltip));

            return html;
        }
    }
}
