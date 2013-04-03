using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Search.Models
{
    public class FolderIndexInfo
    {
        public TextFolder Folder { get; set; }
        public int IndexedContents { get; set; }
        public bool Rebuilding { get; set; }
    }
}
