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
using System.ServiceModel.Configuration;
using System.Text;

namespace Kooboo.CMS.Modules.CMIS.WcfExtensions
{
    public class ApplyOpenCMISResponseFormatterBehaviorElement : BehaviorExtensionElement
    {
        protected override object CreateBehavior()
        {
            return new ApplyOpenCMISResponseFormatterBehavior();
        }

        public override Type BehaviorType
        {
            get { return typeof(ApplyOpenCMISResponseFormatterBehavior); }
        }
    }
}
