using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;
using System.IO;

namespace Kooboo.CMS.Sites.Services
{
    public class Size
    {
        public float Width { get; set; }
        public float Height { get; set; }
    }
    public class HeaderBackgroundManager
    {
        public virtual bool IsEanbled(Theme theme)
        {
            return false;
        }
        public virtual void Change(Theme theme, string originalFile, int x, int y)
        {

        }
        public virtual string GetVirutalPath(Theme theme)
        {
            return "";
        }
        public virtual Size GetContainerSize(Kooboo.CMS.Sites.Models.Site site)
        {
            return new Size();
        }
    }
}
