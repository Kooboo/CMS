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
using System.Collections.Specialized;
using Kooboo.CMS.Search.Models;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;

namespace Kooboo.CMS.Search
{
    public interface IObjectConverter
    {
        KeyValuePair<string, string> GetKeyField(object o);

        IndexObject GetIndexObject(object o);

        object GetNativeObject(NameValueCollection fields);

        string GetUrl(object nativeObject);
    }

    public class ObjectConverters
    {
        public static IObjectConverter GetConverter(Type type)
        {
            return EngineContext.Current.Resolve<IObjectConverter>(type.FullName);
        }
    }
}
