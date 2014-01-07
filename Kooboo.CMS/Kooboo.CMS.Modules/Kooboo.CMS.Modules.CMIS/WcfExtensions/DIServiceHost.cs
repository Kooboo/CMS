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
using System.ServiceModel;
using System.Text;

namespace Kooboo.CMS.Modules.CMIS.WcfExtensions
{
    public class DIServiceHost : ServiceHost
    {
        public DIServiceHost(Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
        }

        protected override void OnOpening()
        {
            base.OnOpening();
            if (this.Description.Behaviors.Find<DIServiceBehaviorAttribute>() == null)
            {
                this.Description.Behaviors.Add(new DIServiceBehaviorAttribute());
            }

            //// Apply query string formatter
            //foreach (ServiceEndpoint endpoint in this.Description.Endpoints)
            //{
            //    foreach (OperationDescription operationDescription in endpoint.Contract.Operations)
            //    {
            //        ApplyOpenCMISResponseFormatterBehavior.ReplaceFormatterBehavior(operationDescription, endpoint.Address);
            //    }
            //}


        }
    }
}
