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
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace Kooboo.CMS.Modules.CMIS.WcfExtensions
{
    public class ApplyOpenCMISResponseFormatterBehavior : IEndpointBehavior//, IContractBehavior
    {
        #region IEndpointBehavior
        public void AddBindingParameters(ServiceEndpoint serviceEndpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        void IEndpointBehavior.ApplyClientBehavior(ServiceEndpoint serviceEndpoint, ClientRuntime behavior)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint serviceEndpoint, EndpointDispatcher endpointDispatcher)
        {
            if (serviceEndpoint == null)
                throw new ArgumentNullException("serviceEndpoint");

            if (endpointDispatcher == null)
                throw new ArgumentNullException("endpointDispatcher");

            //serviceEndpoint.Contract.Behaviors.Add(this);

            foreach (OperationDescription operationDescription in serviceEndpoint.Contract.Operations)
            {
                ReplaceFormatterBehavior(operationDescription, serviceEndpoint.Address);
            }
        }
        public void Validate(ServiceEndpoint serviceEndpoint)
        {
        }

        public static void ReplaceFormatterBehavior(OperationDescription operationDescription, EndpointAddress address)
        {
            // look for and remove the DataContract behavior if it is present
            IOperationBehavior formatterBehavior = operationDescription.Behaviors.Remove<DataContractSerializerOperationBehavior>();
            if (formatterBehavior == null)
            {
                // look for and remove the XmlSerializer behavior if it is present
                formatterBehavior = operationDescription.Behaviors.Remove<XmlSerializerOperationBehavior>();
                if (formatterBehavior == null)
                {
                    // look for delegating formatter behavior
                    DelegatingFormatterBehavior existingDelegatingFormatterBehavior = operationDescription.Behaviors.Find<DelegatingFormatterBehavior>();
                    if (existingDelegatingFormatterBehavior == null)
                    {
                        throw new InvalidOperationException("Could not find DataContractFormatter or XmlSerializer on the contract");
                    }
                }
            }

            //remember what the innerFormatterBehavior was
            DelegatingFormatterBehavior delegatingFormatterBehavior = new DelegatingFormatterBehavior(address, formatterBehavior);
            operationDescription.Behaviors.Add(delegatingFormatterBehavior);
        }
        #endregion

        //#region IContractBehavior

        //public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        //{

        //}

        //public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        //{

        //}

        //public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        //{
        //    dispatchRuntime.OperationSelector = new DispatchByBodyElementOperationSelector();
        //}

        //public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        //{

        //}
        //#endregion
    }

    class DelegatingFormatterBehavior : IOperationBehavior
    {
        EndpointAddress endpointAddress;
        IOperationBehavior innerFormatterBehavior;
        public DelegatingFormatterBehavior(EndpointAddress address, IOperationBehavior innerFormatterBehavior)
        {
            this.endpointAddress = address;
            this.innerFormatterBehavior = innerFormatterBehavior;
        }



        #region IOperationBehavior Members

        public void AddBindingParameters(OperationDescription description, System.ServiceModel.Channels.BindingParameterCollection parameters)
        {
            if (innerFormatterBehavior != null)
            {
                innerFormatterBehavior.AddBindingParameters(description, parameters);
            }
        }

        public void ApplyClientBehavior(OperationDescription description, ClientOperation runtime)
        {
            if (innerFormatterBehavior != null && runtime.Formatter == null)
            {
                // no formatter has been applied yet
                innerFormatterBehavior.ApplyClientBehavior(description, runtime);
            }
        }


        public void ApplyDispatchBehavior(OperationDescription description, DispatchOperation runtime)
        {
            if (innerFormatterBehavior != null && runtime.Formatter == null)
            {
                // no formatter has been applied yet
                innerFormatterBehavior.ApplyDispatchBehavior(description, runtime);
            }
            runtime.Formatter = new OpenCMISResponseFormatter(runtime.Formatter);
        }

        public void Validate(OperationDescription description)
        {
            if (innerFormatterBehavior != null)
            {
                innerFormatterBehavior.Validate(description);
            }
        }

        #endregion

    }
}
