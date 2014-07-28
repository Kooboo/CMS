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
using System.Web.Mvc;

namespace Kooboo.CMS.Common.Extension.TopBar
{
    public class TopBarContext
    {
        public TopBarContext(ControllerContext controllerContext, object optionModel, IEnumerable<object> dataItems)
        {
            this.ControllerContext = controllerContext;
            this.OptionModel = optionModel;
            this.DataItems = dataItems;
        }
        public ControllerContext ControllerContext { get; private set; }

        public object OptionModel { get; private set; }

        public IEnumerable<object> DataItems { get; private set; }

    }
}
