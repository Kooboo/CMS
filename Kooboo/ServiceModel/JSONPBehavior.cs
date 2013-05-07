#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;


namespace System.ServiceModel.Web
{

    /// <summary>
    /// 
    /// </summary>
    public class JSONPBehavior : Attribute, IOperationBehavior
    {
        #region CONST
        /// <summary>
        /// 
        /// </summary>
        public const string DefaultCallback = "callback"; 
        #endregion

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="JSONPBehavior" /> class.
        /// </summary>
        public JSONPBehavior()
            : this(DefaultCallback)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JSONPBehavior" /> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public JSONPBehavior(string callback)
        {
            Callback = callback;
        } 
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the callback.
        /// </summary>
        /// <value>
        /// The callback.
        /// </value>
        public string Callback { get; set; } 
        #endregion

        #region IOperationBehavior Members

        public void AddBindingParameters(
          OperationDescription operationDescription, BindingParameterCollection bindingParameters
        )
        { return; }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            clientOperation.ParameterInspectors.Add(new Inspector(Callback));
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.ParameterInspectors.Add(new Inspector(Callback));
        }

        public void Validate(OperationDescription operationDescription) { return; }

        #endregion

        #region Inspector

        //Parameter inspector
        class Inspector : IParameterInspector
        {
            string callback;
            public Inspector(string callback)
            {
                this.callback = callback;
            }

            public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
            {
            }

            public object BeforeCall(string operationName, object[] inputs)
            {
                string methodName = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters[callback];
                if (methodName != null)
                {
                    //System.ServiceModel.OperationContext.Current.OutgoingMessageProperties["wrapper"] = inputs[0];
                    JSONPMessageProperty property = new JSONPMessageProperty()
                    {
                        MethodName = methodName
                    };
                    OperationContext.Current.OutgoingMessageProperties.Add(JSONPMessageProperty.Name, property);
                }
                return null;
            }
        }
        #endregion

    }
}
