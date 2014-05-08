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

namespace Kooboo.CMS.Sites.DataSource.ValueProvider
{
    public class DictionaryValueProvider<T> : IValueProvider
    {
        public DictionaryValueProvider(IDictionary<string, T> dic)
        {
            this.Dictionary = dic;
        }

        public IDictionary<string, T> Dictionary { get; private set; }

        public object GetValue(string name)
        {
            if (Dictionary != null)
            {
                if (Dictionary.ContainsKey(name))
                {
                    var value = Dictionary[name];
                    return value == null ? null : value.ToString();
                }
            }
            return null;
        }
    }
}
