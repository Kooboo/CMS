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
using System.ServiceModel;
using System.ServiceModel.Channels;
using Kooboo.CMS.Content.FileServer.Interfaces;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Net;

namespace Kooboo.CMS.Content.Persistence.FileServerProvider
{
    public class AuthorizationHeaderInspector : IClientMessageInspector
    {
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {

        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            HttpRequestMessageProperty httpRequestMessage;
            object httpRequestMessageObject;
            if (request.Properties.TryGetValue(HttpRequestMessageProperty.Name, out httpRequestMessageObject))
            {
                httpRequestMessage = httpRequestMessageObject as HttpRequestMessageProperty;
                httpRequestMessage.Headers[HttpRequestHeader.Authorization] = FileServerProviderSettings.Instance.AccountName + "/" + FileServerProviderSettings.Instance.AccountKey;
            }
            else
            {
                httpRequestMessage = new HttpRequestMessageProperty();
                request.Properties.Add(HttpRequestMessageProperty.Name, httpRequestMessage);
            }
            httpRequestMessage.Headers[HttpRequestHeader.Authorization] = FileServerProviderSettings.Instance.AccountName + "/" + FileServerProviderSettings.Instance.AccountKey;
            return null;

        }
    }
    public class HookServiceBehaviour : IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new AuthorizationHeaderInspector());
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.EndpointDispatcher endpointDispatcher)
        {

        }

        public void Validate(ServiceEndpoint endpoint)
        {

        }
    }
    public static class RemoteServiceFactory
    {
        static readonly Dictionary<Type, string> serviceEndpoints = new Dictionary<Type, string>();
        static RemoteServiceFactory()
        {
            serviceEndpoints.Add(typeof(IMediaFolderService), "MediaFolderService");
            serviceEndpoints.Add(typeof(IMediaContentService), "MediaContentService");
            serviceEndpoints.Add(typeof(ITextContentFileService), "TextContentFileService");
        }
        public static T CreateService<T>()
        {
            Binding binding = new WebHttpBinding() { MaxReceivedMessageSize = 2147483647, MaxBufferSize = 2147483647 };
            EndpointAddress address = new EndpointAddress(GetAddress<T>());
            ServiceEndpoint serviceEndpoint = new ServiceEndpoint(ContractDescription.GetContract(typeof(T)), binding, address);
            ChannelFactory<T> factory = new ChannelFactory<T>(serviceEndpoint);
            factory.Endpoint.Behaviors.Add(new WebHttpBehavior());
            factory.Endpoint.Behaviors.Add(new HookServiceBehaviour());

            return factory.CreateChannel();
        }
        private static string GetAddress<T>()
        {
            var path = serviceEndpoints[typeof(T)];
            return FileServerProviderSettings.Instance.Endpoint + path;
        }
    }
}
