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
using Kooboo.Common.Data;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Web.Areas.Contents.Models
{
    public class SelectableViewModel
    {
        public IEnumerable<TextFolder> ChildFolders { get; set; }

        public IEnumerable<TextContent> Selected { get; set; }

        public PagedList<TextContent> Contents { get; set; }

        public bool SingleChoice { get; set; }

        public bool ShowTreeStyle { get; set; }
    }
}