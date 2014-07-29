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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.SiteKernel.SiteFlow
{
    /// <summary>
    /// 处理页面请求
    /// </summary>
    public class PageRequestHandler : IRequestHandler
    {
        //#region .ctor
        //PageExecutor _pageExecutor;
        //public PageRequestHandler(PageExecutor pageExecutor)
        //{
        //    Contract.Requires(pageExecutor != null);

        //    this._pageExecutor = pageExecutor;
        //}
        //#endregion

        //#region Properties
        //public PageExecutor PageExecutor
        //{
        //    get { return _pageExecutor; }
        //}
        //#endregion

        #region Methods
        /// <summary>
        /// Executes the request.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void ExecuteRequest(ControllerContext controllerContext)
        {
            Contract.Requires(controllerContext != null);


        }
        #endregion

    }
}
