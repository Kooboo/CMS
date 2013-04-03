using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;



namespace System.ServiceModel.Web
{
    public class JSONPBindingElement : MessageEncodingBindingElement
    {
        WebMessageEncodingBindingElement wme = new WebMessageEncodingBindingElement();

        public override MessageEncoderFactory CreateMessageEncoderFactory()
        {
            return new JSONPEncoderFactory();
        }

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

        public override BindingElement Clone()
        {
            return new JSONPBindingElement();
        }

        public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("Context is null.");

            context.BindingParameters.Add(this);
            return context.BuildInnerChannelFactory<TChannel>();
        }

        public override bool CanBuildChannelFactory<TChannel>(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("Context is null.");

            return context.CanBuildInnerChannelFactory<TChannel>();
        }

        public override IChannelListener<TChannel> BuildChannelListener<TChannel>(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("Context is null.");

            context.BindingParameters.Add(this);
            return context.BuildInnerChannelListener<TChannel>();
        }

        public override bool CanBuildChannelListener<TChannel>(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("Context is null.");

            context.BindingParameters.Add(this);
            return context.CanBuildInnerChannelListener<TChannel>();
        }

        public override T GetProperty<T>(BindingContext context)
        {
            return wme.GetProperty<T>(context);
        }
    }


}
