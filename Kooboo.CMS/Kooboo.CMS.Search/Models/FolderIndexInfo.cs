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
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Search.Models
{
    public class FolderIndexInfo
    {
        public string FolderName { get; set; }
        public int IndexedContents { get; set; }
        public bool Rebuilding { get; set; }
    }
}
