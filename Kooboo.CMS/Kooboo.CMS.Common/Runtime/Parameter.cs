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

namespace Kooboo.CMS.Common.Runtime
{
    public class Parameter
    {
        public Parameter(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }
        public string Name { get; set; }
        public object Value { get; set; }
    }
}
