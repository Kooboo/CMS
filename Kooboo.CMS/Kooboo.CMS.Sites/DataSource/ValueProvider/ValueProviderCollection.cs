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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.DataSource.ValueProvider
{
    public class ValueProviderCollection : Collection<IValueProvider>, IValueProvider
    {
        public ValueProviderCollection()
        {

        }
        public ValueProviderCollection(IList<IValueProvider> list)
            : base(list)
        {

        }
        public object GetValue(string name)
        {
            object value = null;
            foreach (var item in this)
            {
                value = item.GetValue(name);
                if (value != null)
                {
                    break;
                }
            }
            return value;
        }
    }
}
