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
    public class MultiFiles : Input
    {
        public override string Name
        {
            get { return "MultiFiles"; }
        }
        public override string Type
        {
            get { return "file"; }
        }
        protected override string RenderInput(IColumn column)
        {
            return string.Format(@"<input id=""{0}"" name=""{0}"" type=""{1}"" value=""@(Model.{0} ?? """")""  displayValue=""@(Model.{0} ?? """")"" {2} multiple=""multiple""/>", column.Name, Type, Kooboo.CMS.Form.Html.ValidationExtensions.GetUnobtrusiveValidationAttributeString(column));
        }
        public override bool IsFile
        {
            get
            {
                return true;
            }
        }        
    }
}
