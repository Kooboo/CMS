using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.View;
using System.Web.Mvc;
using System.Collections.Specialized;

namespace Kooboo.CMS.Sites.DataRule
{
    public class DataRuleContext
    {
        public DataRuleContext(Site site, Page page)
        {
            this.Site = site;
            this.Page = page;         
        }
        public Site Site { get; private set; }
        public Page Page { get; private set; }
        public IValueProvider ValueProvider { get; set; }
    }
}
