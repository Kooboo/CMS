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
    }
}
