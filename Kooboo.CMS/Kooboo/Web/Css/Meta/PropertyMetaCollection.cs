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
    public class PropertyMetaCollection : Dictionary<string, PropertyMeta>
    {
        public void Add(PropertyMeta meta)
        {
            Add(meta.Name, meta);

            if (meta.IsShorthand)
            {
                foreach (var each in meta.SubProperties)
                {
                    if (ContainsKey(each))
                    {
                        var subMeta = this[each];
                        subMeta.ShorthandName = meta.Name;
                    }
                }
            }
        }
    }
}
