using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Form.Html.Controls
{
    public class CheckBoxList : ControlBase
    {
        public override string Name
        {
            get { return "CheckBoxList"; }
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
            StringBuilder sb = new StringBuilder(string.Format(@"@{{ var checkBoxListDefaultValue_{0} = @""{1}"".Split(new []{{','}},StringSplitOptions.RemoveEmptyEntries);
                        var values_{0} = new string[0];
                        if(!string.IsNullOrEmpty(Model.{0}))
                        {{
                            values_{0}=Model.{0}.Split(new []{{','}},StringSplitOptions.RemoveEmptyEntries);
                        }}
                        }}", column.Name, column.DefaultValue.EscapeQuote()));

            if (!string.IsNullOrEmpty(column.SelectionFolder))
            {
                sb.AppendFormat(@"
                        @{{
                           var textFolder_{0} = new TextFolder(Repository.Current, ""{1}"");
                           var query_{0} = textFolder_{0}.CreateQuery().DefaultOrder(); 
                           var index_{0} = 0;
                        }}
                        <ul class=""checkbox-list"">
                        @foreach (var item in query_{0})
                        {{                            
                            var id = ""{1}"" + index_{0}.ToString();
                            <li>
                             <input id=""@id"" name=""{0}"" type=""checkbox"" value=""@item.UUID""  @((Model.{0} == null && checkBoxListDefaultValue_{0}.Contains(@item.UUID, StringComparer.OrdinalIgnoreCase)) || (Model.{0} != null && values_{0}.Contains(@item.UUID, StringComparer.OrdinalIgnoreCase)) ? ""checked"" : """")/><label
                            for=""@id"">@item.GetSummary()</label>
                            </li>
                            index_{0}++;                            
                        }}
                        </ul>
                        ", column.Name, column.SelectionFolder);
            }
            else
            {
                if (column.SelectionItems != null)
                {
                    var index = 0;
                    sb.Append(@"<ul class=""checkbox-list"">");
                    foreach (var item in column.SelectionItems)
                    {
                        var id = column.Name + "_" + index.ToString();
                        index++;
                        sb.AppendFormat(@"
<li>
<input id=""{0}"" name=""{1}"" type=""checkbox"" value=""@(@""{2}"")""  @((Model.{1} == null && checkBoxListDefaultValue_{1}.Contains(@""{2}"",StringComparer.OrdinalIgnoreCase))  || (Model.{1} != null && values_{1}.Contains(@""{2}"",StringComparer.OrdinalIgnoreCase)) ? ""checked"" : """")/><label for=""{0}"">{3}</label>
</li>"
                            , id, column.Name, item.Value.EscapeQuote(), item.Text);
                    }
                    sb.Append("</ul>");
                }
            }


            return sb.ToString();
        }
    }
}
