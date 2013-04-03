using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Form.Html.Controls
{
    public class File : Input
    {
        public override string Name
        {
            get { return "File"; }
        }

        public override string Type
        {
            get { return "file"; }
        }
        public override bool IsFile
        {
            get
            {
                return true;
            }
        }
        protected override string RenderInput(IColumn column)
        {
            return string.Format(@"<input id=""{0}"" name=""{0}"" type=""{1}"" value=""@(Model.{0} ?? """")""  displayValue=""@(Model.{0} ?? """")"" {2}/>", column.Name, Type, Kooboo.CMS.Form.Html.ValidationExtensions.GetUnobtrusiveValidationAttributeString(column));
        }
    }
}
