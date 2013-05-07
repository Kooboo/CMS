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
using System.Web;

namespace Kooboo.CMS.Form.Html.Controls
{
    public class InputFloat : ControlBase
    {
        #region IControl Members

        public override string Name
        {
            get
            {
                return "Float";
            }
        }
        protected override string RenderInput(IColumn column)
        {

            return string.Format(@"<input class=""long numeric"" id=""{0}"" name=""{0}"" type=""text"" value=""@(Model.{0} ?? """")"" {1} value-type=""{2}""/>", column.Name, Kooboo.CMS.Form.Html.ValidationExtensions.GetUnobtrusiveValidationAttributeString(column), "float");
        }


        #endregion
    }
}
