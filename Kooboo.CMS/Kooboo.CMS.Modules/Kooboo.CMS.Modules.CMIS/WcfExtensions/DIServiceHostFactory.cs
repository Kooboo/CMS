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
using System.ServiceModel.Activation;
using System.Text;

namespace Kooboo.CMS.Modules.CMIS.WcfExtensions
{
    public class DIServiceHostFactory : ServiceHostFactoryBase
    {
        public override System.ServiceModel.ServiceHostBase CreateServiceHost(string constructorString, Uri[] baseAddresses)
        {
            var serviceType = Type.GetType(constructorString);

            return new DIServiceHost(serviceType, baseAddresses);
        }
    }
}
