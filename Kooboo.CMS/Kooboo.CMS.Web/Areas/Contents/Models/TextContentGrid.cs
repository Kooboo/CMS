using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Sites.DataRule;

namespace Kooboo.CMS.Web.Areas.Contents.Models
{
    public class TextContentGrid
    {
        public IEnumerable<TextFolder> ChildFolders { get; set; }
        public IContentQuery<TextContent> ContentQuery
        {
            get;
            set;
        }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public bool ShowTreeStyle { get; set; }
        [Obsolete]
        public bool HasConditions { get { return !ShowTreeStyle; } }
        //public TextFolder TextFolder { get; set; }
        //public Schema Schema { get; set; }
        //public string SchemaView { get; set; }
        //public IEnumerable<WhereClause> WhereClauses { get; set; }
    }
}
