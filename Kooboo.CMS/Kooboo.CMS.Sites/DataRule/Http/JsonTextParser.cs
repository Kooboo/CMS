#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.DataRule.Http
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IResponseTextParser), Key = "JsonTextParser")]
    public class JsonTextParser : IResponseTextParser
    {
        public bool Accept(string responseText, string contentType)
        {
            contentType = (contentType ?? "").ToLower();
            return contentType.Contains("application/json") || (responseText.StartsWith("{") && responseText.EndsWith("}")) || (responseText.StartsWith("[") && responseText.EndsWith("]"));
        }

        public dynamic Parse(string responseText)
        {
            return JsonConvert.DeserializeObject<dynamic>(responseText);
        }
    }
}
