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
        TextContent Update(Schema schema, TextContent textContent, NameValueCollection values);

        TextContent Default(Schema schema);

        object ConvertToColumnType(Schema schema, string field, string value);
    }
}
