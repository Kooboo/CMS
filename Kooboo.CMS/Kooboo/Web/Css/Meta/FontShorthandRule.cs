﻿#region License
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
    public class FontShorthandRule : ValueDiscriminationRule
    {
        public FontShorthandRule()
            : base("[font-style font-variant font-weight] font-size/line-height font-family")
        { 
        }

        public override bool TryCombine(IEnumerable<Property> properties, PropertyMeta meta, out Property property)
        {
            property = null;
            return false;
        }
    }
}
