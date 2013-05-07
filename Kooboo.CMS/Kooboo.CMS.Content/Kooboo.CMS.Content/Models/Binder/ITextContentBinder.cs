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

namespace Kooboo.CMS.Content.Models.Binder
{
    public interface ITextContentBinder
    {
        TextContent Bind(Schema schema, TextContent textContent, NameValueCollection values);
        TextContent Bind(Schema schema, TextContent textContent, System.Collections.Specialized.NameValueCollection values, bool thorwViolationException);
        TextContent Bind(Schema schema, TextContent textContent, System.Collections.Specialized.NameValueCollection values, bool update, bool thorwViolationException);

        TextContent Update(Schema schema, TextContent textContent, NameValueCollection values);
        TextContent Default(Schema schema);
        object ConvertToColumnType(Schema schema, string field, string value);
    }
}
