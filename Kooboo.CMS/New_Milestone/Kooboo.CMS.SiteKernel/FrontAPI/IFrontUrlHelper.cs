#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.SiteKernel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.SiteKernel.FrontAPI
{
    public interface IFrontUrlHelper
    {
        UrlHelper UrlHelper { get; }
        Site ISite { get; }
        FrontRequestChannel FrontRequestChannel { get; }
        IHtmlString WrapperUrl(string url);
        IHtmlString WrapperUrl(string url, bool? requireSSL);
    }
}
