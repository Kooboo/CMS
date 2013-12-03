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
    public class DropDownList : ControlBase
    {
        public override string Name
        {
            get { return "DropDownList"; }
        }
        public override bool HasDataSource
        {
            get
            {
                return true;
            }
        }

        protected override string RenderInput(IColumn column)
        {
            StringBuilder sb = new StringBuilder(string.Format(@"@{{ var dropDownDefault_{0} =  @""{1}"";}}
                <select name=""{0}"" class=""long"">", column.Name, column.DefaultValue.EscapeQuote()));

            if (column.SelectionSource == SelectionSource.TextFolder)
            {
                string emptyOption = "";
                if (column.AllowNull)
                {
                    emptyOption = @"<option value=""""></option>";
                }
                sb.AppendFormat(@"
                        @{{
                            var textFolder_{0} = new TextFolder(Repository.Current, ""{1}"");
                            var query_{0} = textFolder_{0}.CreateQuery().DefaultOrder();
                        }}
                        {2}
                        @foreach (var item in query_{0})
                        {{                            
                            <option value=""@item.UUID"" @((Model.{0} != null && Model.{0}.ToString().ToLower() == @item.UUID.ToLower()) || (Model.{0} == null && dropDownDefault_{0}.ToLower() == @item.UUID.ToLower()) ? ""selected"" : """")>@item.GetSummary()</option>
                        }}
                        ", column.Name, column.SelectionFolder, emptyOption);
            }
            else
            {
                if (column.SelectionItems != null)
                {
                    foreach (var item in column.SelectionItems)
                    {
                        sb.AppendFormat(@"
                        <option value=""@(@""{1}"")"" @((Model.{0} != null && Model.{0}.ToString().ToLower() == @""{1}"".ToLower()) || (Model.{0} == null && dropDownDefault_{0}.ToLower() == @""{1}"".ToLower()) ? ""selected"" : """")>{2}</option>"
                            , column.Name, item.Value.EscapeQuote(), item.Text);
                    }
                }
            }

            sb.Append("</select>");

            return sb.ToString();
        }
    }
}
