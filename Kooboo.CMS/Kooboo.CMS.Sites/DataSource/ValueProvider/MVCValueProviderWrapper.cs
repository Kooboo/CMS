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
    public class MVCValueProviderWrapper : IValueProvider
    {
        System.Web.Mvc.IValueProvider valueProvider;
        public MVCValueProviderWrapper(System.Web.Mvc.IValueProvider valueProvider)
        {
            this.valueProvider = valueProvider;
        }

        public object GetValue(string name)
        {
            var result = valueProvider.GetValue(name);
            if (result != null)
            {
                return result.RawValue;
            }
            return null;
        }
    }
}
