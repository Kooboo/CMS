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

namespace Kooboo.Web.Css.Meta
{
    public class BorderPositionShorthandRule : PositionShorthandRule
    {
        public static readonly Dictionary<string, PropertyValueType> BorderProperties = new Dictionary<string, PropertyValueType>()
        {
            { "width", PropertyValueType.Length },
            { "style", new EnumType("none | hidden | dotted | dashed | solid | double | groove | ridge | inset | outset") },
            { "color", PropertyValueType.Color }
        };

        protected override string CombineName(PropertyMeta meta, string name)
        {
            string[] splitted = meta.Name.Split('-');
            return splitted[0] + "-" + name + "-" + splitted[1];
        }
    }
}
