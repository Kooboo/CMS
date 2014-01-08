using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence.EntityFramework.SiteProvider
{
    public interface ISiteSettingEntity
    {
        string ParentName { get; set; }

        string FullName { get; set; }

        string ObjectXml { get; set; }
    }

    public class SiteEntity : ISiteSettingEntity
    {
        public string FullName { get; set; }
        
        public string ParentName { get; set; }

        public string ObjectXml { get; set; }
    }
}
