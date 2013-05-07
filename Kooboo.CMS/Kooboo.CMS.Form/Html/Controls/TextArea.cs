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
    public class TextArea : ControlBase
    {
        public override string Name
        {
            get { return "TextArea"; }
        }

        protected override string RenderInput(IColumn column)
        {
            return string.Format(@"<textarea class=""extra-large"" name=""{0}"" {1}>@(Model.{0} ?? """")</textarea> ", column.Name, Kooboo.CMS.Form.Html.ValidationExtensions.GetUnobtrusiveValidationAttributeString(column));
        }
    }
}
