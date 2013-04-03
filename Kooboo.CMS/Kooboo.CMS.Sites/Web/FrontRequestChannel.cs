using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Web
{
    public enum FrontRequestChannel
    {
        Unknown,
        /// <summary>
        /// s~site1
        /// </summary>
        Debug,
        /// <summary>
        /// www.site1.com
        /// </summary>
        Host,
        /// <summary>
        /// www.kooboo.com/site1
        /// </summary>
        HostNPath,
        /// <summary>
        /// 
        /// </summary>
        Design,
        Draft
    }
}
