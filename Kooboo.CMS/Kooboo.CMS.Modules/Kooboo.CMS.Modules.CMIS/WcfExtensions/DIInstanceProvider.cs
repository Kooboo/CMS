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
using System.ServiceModel.Dispatcher;
using System.Text;

namespace Kooboo.CMS.Modules.CMIS.WcfExtensions
{
    public class DIInstanceProvider : IInstanceProvider
    {
        Kooboo.CMS.Common.Runtime.IEngine _engine;
        public Type ContractType { get; private set; }
        public DIInstanceProvider(Kooboo.CMS.Common.Runtime.IEngine engine, Type contractType)
        {
            _engine = engine;
            ContractType = contractType;
        }
        public object GetInstance(System.ServiceModel.InstanceContext instanceContext, System.ServiceModel.Channels.Message message)
        {
            return this._engine.Resolve(this.ContractType);
        }

        public object GetInstance(System.ServiceModel.InstanceContext instanceContext)
        {
            return this.GetInstance(instanceContext, null);
        }

        public void ReleaseInstance(System.ServiceModel.InstanceContext instanceContext, object instance)
        {

        }
    }
}
