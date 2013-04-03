using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Form.Html
{
    public interface ISchemaForm
    {
        string Generate(ISchema schema);
    }
}
