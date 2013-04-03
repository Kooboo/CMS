using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Form.Html.Controls
{
    public class CheckBox : ControlBase
    {
        public override string Name
        {
            get { return "CheckBox"; }
        }

        protected override string RenderInput(IColumn column)
        {
            return string.Format(@"<input name=""{0}"" type=""checkbox"" @(Convert.ToBoolean(Model.{0})?""checked"":"""") value=""true""/>
                                    <input type=""hidden"" value=""false"" name=""{0}""/>", column.Name);
        }
    }
}
