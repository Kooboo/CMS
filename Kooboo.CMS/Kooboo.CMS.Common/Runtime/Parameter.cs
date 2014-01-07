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
            this.ValueCallback = () => value;
        }
        public Parameter(string name, Func<object> value)
        {
            this.Name = name;
            this.ValueCallback = value;
        }
        public string Name { get; private set; }
        public Func<object> ValueCallback { get; private set; }
    }
}
