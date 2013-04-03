using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.CMS.Form.Html.Controls
{
    public class InputNumber : ControlBase
    {
        #region IControl Members

        public override string Name
        {
            get
            {
                return "Number";
            }
        }
        protected override string RenderInput(IColumn column)
        {
            
            return string.Format(@"<input id=""{0}"" name=""{0}"" type=""{1}"" value=""@(Model.{0} ?? """")"" {2} value-type=""{3}""/>", column.Name, Name, Kooboo.CMS.Form.Html.ValidationExtensions.GetUnobtrusiveValidationAttributeString(column),"float");
        }


        #endregion
    }
}
