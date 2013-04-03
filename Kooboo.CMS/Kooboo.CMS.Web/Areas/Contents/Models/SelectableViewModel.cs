using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Paging;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Web.Areas.Contents.Models
{
    public class SelectableViewModel
    {
        public IEnumerable<TextFolder> ChildFolders { get; set; }

        public IEnumerable<TextContent> Selected { get; set; }

        public PagedList<TextContent> Contents { get; set; }

        public bool SingleChoice { get; set; }
    }
}