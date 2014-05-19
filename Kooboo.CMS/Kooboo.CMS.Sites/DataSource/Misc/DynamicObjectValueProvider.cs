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
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.DataSource.Misc
{
    public class DynamicObjectValueProvider : IValueProvider
    {
        DynamicObject _dynamicObject;
        public DynamicObjectValueProvider(DynamicObject dynamicObject)
        {
            _dynamicObject = dynamicObject;
        }
        public bool ContainsPrefix(string prefix)
        {
            return false;
        }

        public ValueProviderResult GetValue(string key)
        {
            object result;

            _dynamicObject.TryGetMember(new GetMemberBinderWrapper(key), out result);

            return new ValueProviderResult(result, result == null ? null : result.ToString(), System.Globalization.CultureInfo.CurrentCulture);
        }
    }
}
