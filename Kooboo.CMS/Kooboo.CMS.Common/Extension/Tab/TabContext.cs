#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Kooboo.CMS.Common.Extension.Tab
{
    public class TabContext
    {
        private DynamicDictionary dynamicDictionary;
        public TabContext(ControllerContext controllerContext, ViewDataDictionary viewData)
        {
            this.ControllerContext = controllerContext;
            this.ViewData = viewData ?? new ViewDataDictionary();
            dynamicDictionary = new DynamicDictionary(this.ViewData);
        }
        public ControllerContext ControllerContext { get; private set; }
        public ViewDataDictionary ViewData { get; private set; }
        public dynamic ViewBag
        {
            get
            {
                return dynamicDictionary;
            }
        }
        public object Model
        {
            get
            {
                return ViewData.Model;
            }
            set
            {
                ViewData.Model = value;
            }
        }
    }
}
