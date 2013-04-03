using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.ComponentModel;
using Kooboo.CMS.Account.Models;
using Kooboo.CMS.Web.Areas.Account.Models;
using Kooboo.CMS.Sites;
//using Kooboo.CMS.Web.ErrorHandling;

namespace Kooboo.CMS.Web.Areas.Account.Controllers
{
    //[CmsHandleError(Order = 99)]
    public abstract class ControllerBase : AreaControllerBase
    {
        static ControllerBase()
        {
            TypeDescriptorHelper.RegisterMetadataType(typeof(Role), typeof(Role_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(User), typeof(User_Metadata));
            TypeDescriptorHelper.RegisterMetadataType(typeof(Setting), typeof(Setting_Metadata));
        }
    }
}
