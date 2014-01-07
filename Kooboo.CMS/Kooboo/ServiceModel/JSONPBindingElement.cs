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
using System.ServiceModel.Channels;



namespace System.ServiceModel.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class JSONPBindingElement : MessageEncodingBindingElement
    {
        #region Fields
        WebMessageEncodingBindingElement wme = new WebMessageEncodingBindingElement(); 
        #endregion


        #region Properties
        /// <summary>
        /// When overridden in a derived class, gets or sets the message version that can be handled by the message encoders produced by the message encoder factory.
        /// </summary>
        /// <returns>The <see cref="T:System.ServiceModel.Channels.MessageVersion" /> used by the encoders produced by the message encoder factory.</returns>
        /// <exception cref="System.Exception"></exception>
        public override MessageVersion MessageVersion
        {
            get
            {
                return wme.MessageVersion;
            }
            set
            {
                throw new Exception("MessageVersion property cannot be set.");
            }
        } 
        #endregion

        #region Methods

        /// <summary>
        /// When overridden in a derived class, creates a factory for producing message encoders.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.ServiceModel.Channels.MessageEncoderFactory" /> used to produce message encoders.
        /// </returns>
        public override MessageEncoderFactory CreateMessageEncoderFactory()
        {
            return new JSONPEncoderFactory();
        }

        /// <summary>
        /// When overridden in a derived class, returns a copy of the binding element object.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ServiceModel.Channels.BindingElement" /> object that is a deep clone of the original.
        /// </returns>
        public override BindingElement Clone()
        {
            return new JSONPBindingElement();
        }

        /// <summary>
        /// Builds the channel factory.
        /// </summary>
        /// <typeparam name="TChannel">The type of the channel.</typeparam>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("Context is null.");

            context.BindingParameters.Add(this);
            return context.BuildInnerChannelFactory<TChannel>();
        }

        /// <summary>
        /// Determines whether this instance [can build channel factory] the specified context.
        /// </summary>
        /// <typeparam name="TChannel">The type of the channel.</typeparam>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if this instance [can build channel factory] the specified context; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public override bool CanBuildChannelFactory<TChannel>(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("Context is null.");

            return context.CanBuildInnerChannelFactory<TChannel>();
        }

        /// <summary>
        /// Builds the channel listener.
        /// </summary>
        /// <typeparam name="TChannel">The type of the channel.</typeparam>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public override IChannelListener<TChannel> BuildChannelListener<TChannel>(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("Context is null.");

            context.BindingParameters.Add(this);
            return context.BuildInnerChannelListener<TChannel>();
        }

        /// <summary>
        /// Determines whether this instance [can build channel listener] the specified context.
        /// </summary>
        /// <typeparam name="TChannel">The type of the channel.</typeparam>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if this instance [can build channel listener] the specified context; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public override bool CanBuildChannelListener<TChannel>(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("Context is null.");

            context.BindingParameters.Add(this);
            return context.CanBuildInnerChannelListener<TChannel>();
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override T GetProperty<T>(BindingContext context)
        {
            return wme.GetProperty<T>(context);
        } 
        #endregion
    }


}
