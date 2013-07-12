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

namespace Kooboo.CMS.Common.Formula
{
    public class MvcValueProvider : IValueProvider
    {
        System.Web.Mvc.IValueProvider _valueProvider;
        public MvcValueProvider(System.Web.Mvc.IValueProvider valueProvider)
        {
            this._valueProvider = valueProvider;
        }
        public object GetValue(string name)
        {
            var result = this._valueProvider.GetValue(name);
            if (result != null)
            {
                return result.AttemptedValue;
            }
            return null;
        }
    }
}
