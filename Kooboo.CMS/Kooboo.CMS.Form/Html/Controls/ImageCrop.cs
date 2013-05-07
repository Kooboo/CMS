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
    public class ImageCrop : Input
    {
        public const string KoobooImageCropInputName = "Kooboo-Image-Crop-Field";

        public override string Type
        {
            get { return "ImageCrop"; }
        }

        public override string Name
        {
            get { return "ImageCrop"; }
        }

        protected override string RenderInput(IColumn column)
        {
            string formater = @"<input class=""medium"" id=""{0}"" name=""{0}"" type=""text"" value=""@(Model.{0} ?? """")"" {1} readonly=""readonly""/>
<a href=""javascript:;"" class=""image-croper action"" inputid = ""{0}"">@Html.IconImage(""plus"")</a>
<input id=""{0}-hidden"" name=""{0}-hidden"" type=""hidden"" value=""@(Model.{0} ?? """")""/>
<input type=""hidden"" name=""{2}"" value=""{0}""/>";

            return string.Format(formater, column.Name, Kooboo.CMS.Form.Html.ValidationExtensions.GetUnobtrusiveValidationAttributeString(column), KoobooImageCropInputName);
        }
    }
}
