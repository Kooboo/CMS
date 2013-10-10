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
using System.ServiceModel.Description;
using System.Text;

namespace Kooboo.CMS.Modules.CMIS.WcfExtensions
{
    /// <summary>
    /// http://msdn.microsoft.com/en-us/library/aa395223%28v=VS.90%29.aspx
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    class DispatchByBodyElementBehaviorAttribute : Attribute, IContractBehavior
    {
        #region IContractBehavior Members

        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            // no binding parameters need to be set here
            return;
        }

        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            // this is a dispatch-side behavior which doesn't require
            // any action on the client
            return;
        }

        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.DispatchRuntime dispatchRuntime)
        {
            dispatchRuntime.OperationSelector =
                new DispatchByBodyElementOperationSelector();
        }

        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
            // 
        }

        #endregion
    }
}
