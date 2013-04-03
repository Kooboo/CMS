using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Kooboo.CMS.Sites.View.CodeSnippet
{
    public interface ILayoutPositionParser
    {
        IEnumerable<string> Parse(string layoutBody);

        IHtmlString RegisterClientParser();
        IHtmlString RegisterClientAddPosition();
        IHtmlString RegisterClientRemovePosition();
    }    
}
