using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
namespace Kooboo.Web.Mvc.Html
{
    public interface IHtmlControl
    {
        IHtmlControl AddClass(string className);
        
        IHtmlControl SetAttribute(string name, string value);
        
        IHtmlControl SetName(string name);
        
        IHtmlControl SetId(string id);

        IDictionary<string, string> Attributes { get;}

        IHtmlControl Html(string html);
        IHtmlControl Text(string text);

        string Name
        {
            get;
        }

        string Id
        {
            get;
        }

        MvcHtmlString ToHtmlString();

    }
}
