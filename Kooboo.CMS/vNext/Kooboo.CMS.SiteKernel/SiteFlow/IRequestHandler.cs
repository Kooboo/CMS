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
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.SiteKernel.SiteFlow
{
    /// <summary>
    /// 处理站点请求的接口
    /// </summary>
    public interface IRequestHandler
    {
        void ExecuteRequest(ControllerContext controllerContext);
    }
}
