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
using System.Web;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.ServiceModel.Configuration;
using System.Net;
using System.ServiceModel.Web;
using System.IdentityModel.Tokens;
using Kooboo;
namespace Kooboo.CMS.Content.FileServer.Web
{
    public class HmacAuthorizationMessageInspector : IDispatchMessageInspector
    {
        static string Username = "";
        static string Password = "";
        static HmacAuthorizationMessageInspector()
        {
            Username = System.Web.Configuration.WebConfigurationManager.AppSettings["AccountName"];
            Password = System.Web.Configuration.WebConfigurationManager.AppSettings["AccountKey"];
        }
        object IDispatchMessageInspector.AfterReceiveRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel, System.ServiceModel.InstanceContext instanceContext)
        {
            var authorizationHeader = WebOperationContext.Current.IncomingRequest.Headers[HttpRequestHeader.Authorization];
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                throw new SecurityTokenException("Username and password required");
            }
            else
            {
                var splitIndex = authorizationHeader.IndexOf("/");
                if (splitIndex != -1 && splitIndex < authorizationHeader.Length - 1)
                {
                    var username = authorizationHeader.Substring(0, splitIndex);
                    var password = authorizationHeader.Substring(splitIndex + 1);
                    if (ValidateUser(username, password))
                    {
                        return null;
                    }
                    else
                    {
                        throw new WebFaultException(System.Net.HttpStatusCode.Unauthorized);
                    }
                }
                else
                {
                    throw new SecurityTokenException("Username and password required");
                }
            }
        }
        private bool ValidateUser(string username, string password)
        {
            return Username.EqualsOrNullEmpty(username, StringComparison.OrdinalIgnoreCase) && Password == password;
        }

        void IDispatchMessageInspector.BeforeSendReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {

        }
    }

    public class HmacAuthorizationServiceBehavior : IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {

        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            HmacAuthorizationMessageInspector inspector = new HmacAuthorizationMessageInspector();

            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(inspector);
        }

        public void Validate(ServiceEndpoint endpoint)
        {

        }
    }

    public class HmacAuthorizationConfigurationSection : BehaviorExtensionElement, IServiceBehavior
    {
        #region IServiceBehavior Members

        public void AddBindingParameters(ServiceDescription serviceDescription,
            System.ServiceModel.ServiceHostBase serviceHostBase,
            System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints,
            System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {

        }

        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {

        }

        #endregion

        public override Type BehaviorType
        {
            get { return typeof(HmacAuthorizationServiceBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new HmacAuthorizationServiceBehavior();
        }
    }

}