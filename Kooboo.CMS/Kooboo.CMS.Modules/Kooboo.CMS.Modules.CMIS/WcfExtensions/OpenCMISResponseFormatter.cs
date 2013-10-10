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
    /// <summary>
    /// 
    /// </summary>
    public class OpenCMISResponseFormatter : IDispatchMessageFormatter
    {
        #region .ctor
        IDispatchMessageFormatter innerDispatchFormatter;
        public OpenCMISResponseFormatter(IDispatchMessageFormatter innerDispatchFormatter)
        {
            if (innerDispatchFormatter == null)
                throw new ArgumentNullException("innerDispatchFormatter");

            this.innerDispatchFormatter = innerDispatchFormatter;

        }
        #endregion

        #region DeserializeRequest
        public void DeserializeRequest(System.ServiceModel.Channels.Message message, object[] parameters)
        {

        }
        #endregion

        #region SerializeReply
        private void FindResult(XmlNode node, out XmlNode resultNode)
        {
            resultNode = null;
            if (IsResultNode(node))
            {
                resultNode = node;
            }
            else
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    FindResult(childNode, out resultNode);
                    if (resultNode != null)
                    {
                        break;
                    }
                }
            }
        }
        private bool IsResultNode(XmlNode node)
        {
            return (node.Name.EndsWith("Result")) && node.ParentNode.Name.EndsWith("Response");
        }
        private Message RemoveResultElement(Message oldMessage)
        {
            XmlDocument xdom = new XmlDocument();
            xdom.LoadXml(oldMessage.ToString());
            var root = xdom.SelectSingleNode("/");

            XmlNode resultNode = null;
            FindResult(root, out resultNode);

            var responseNode = resultNode.ParentNode;

            if (resultNode != null && resultNode.Name.EndsWith("Result"))
            {
                foreach (XmlNode item in resultNode.ChildNodes)
                {
                    responseNode.AppendChild(item);
                }
                responseNode.RemoveChild(resultNode);
            }
            XmlReader reader = new XmlNodeReader(xdom);
            Message newMessage = Message.CreateMessage(reader, int.MaxValue, oldMessage.Version);
            newMessage.Properties.CopyProperties(oldMessage.Properties);
            return newMessage;
        }
        public System.ServiceModel.Channels.Message SerializeReply(System.ServiceModel.Channels.MessageVersion messageVersion, object[] parameters, object result)
        {
            var message = innerDispatchFormatter.SerializeReply(messageVersion, parameters, result);

            return RemoveResultElement(message);
        }
        #endregion
    }
}
