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
    public abstract class Input : ControlBase
    {
        #region IControl Members

        public abstract string Type { get; }
        protected override string RenderInput(IColumn column)
        {
            return string.Format(@"<input class='long' id=""{0}"" name=""{0}"" type=""{1}"" value=""@(Model.{0} ?? """")"" {2}/>", column.Name, Type, Kooboo.CMS.Form.Html.ValidationExtensions.GetUnobtrusiveValidationAttributeString(column));
        }


        #endregion
    }
}
