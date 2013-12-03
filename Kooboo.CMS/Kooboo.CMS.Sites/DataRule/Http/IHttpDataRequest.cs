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
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.DataRule.Http
{
    public interface IHttpDataRequest
    {
        dynamic GetData(string url, string httpMethod, string contentType, NameValueCollection form, NameValueCollection headers);
    }

}
