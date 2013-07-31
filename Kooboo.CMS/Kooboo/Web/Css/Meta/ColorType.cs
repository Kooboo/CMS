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
using System.Drawing;

namespace Kooboo.Web.Css.Meta
{
    public class ColorType : PropertyValueType
    {
        public override string DefaultValue
        {
            get { return System.Drawing.Color.Transparent.Name.ToLower(); }
        }

        public override bool IsValid(string value)
        {
            value = value.ToLower();
            return IsKnownColor(value) || IsRgb(value) || IsRgbHex(value);
        }

        private bool IsKnownColor(string value)
        {
            var color = System.Drawing.Color.FromName(value);
            return color.IsKnownColor;
        }

        private bool IsRgb(string value)
        {
            return value.StartsWith("rgb(") && value.EndsWith(")");
        }

        private bool IsRgbHex(string value)
        {
            int dec;
            return value.StartsWith("#") && Int32.TryParse(value.TrimStart('#'), System.Globalization.NumberStyles.HexNumber, null, out dec);
        }
    }
}
