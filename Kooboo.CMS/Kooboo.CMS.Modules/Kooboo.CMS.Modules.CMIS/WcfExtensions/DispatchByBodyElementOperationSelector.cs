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
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Xml;

namespace Kooboo.CMS.Modules.CMIS.WcfExtensions
{
    class DispatchByBodyElementOperationSelector : IDispatchOperationSelector
    {
        public DispatchByBodyElementOperationSelector()
        {
        }

        #region IDispatchOperationSelector Members

        private Message CreateMessageCopy(Message message, XmlDictionaryReader body)
        {
            Message copy = Message.CreateMessage(message.Version, message.Headers.Action, body);
            copy.Headers.CopyHeaderFrom(message, 0);
            copy.Properties.CopyProperties(message.Properties);
            return copy;
        }

        public string SelectOperation(ref System.ServiceModel.Channels.Message message)
        {
            // When WCF throw "This message cannot support the operation because it has been read" exception, see the following link to fix.
            //http://stackoverflow.com/a/11170390/2814166            
            XmlDictionaryReader bodyReader = message.GetReaderAtBodyContents();
            //XmlQualifiedName lookupQName = new XmlQualifiedName(bodyReader.LocalName, bodyReader.NamespaceURI);
            message = CreateMessageCopy(message, bodyReader);
            return bodyReader.LocalName;
        }

        #endregion
    }
}
