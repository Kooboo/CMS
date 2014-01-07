#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Text;
using System.ServiceModel.Channels;
using System.IO;
using System.Xml;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;


namespace System.ServiceModel.Web
{

    #region JSONPMessageProperty
    /// <summary>
    /// 
    /// </summary>
    public class JSONPMessageProperty : IMessageProperty
    {
        #region CONST
        /// <summary>
        /// 
        /// </summary>
        public const string Name = "Microsoft.ServiceModel.Samples.JSONPMessageProperty";
        #endregion

        #region Methods
        /// <summary>
        /// Creates a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ServiceModel.Channels.IMessageProperty" /> object that is a copy of the current instance.
        /// </returns>
        public IMessageProperty CreateCopy()
        {
            return new JSONPMessageProperty(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JSONPMessageProperty" /> class.
        /// </summary>
        public JSONPMessageProperty()
        {
        }

        internal JSONPMessageProperty(JSONPMessageProperty other)
        {
            this.MethodName = other.MethodName;
        }
        #endregion

        #region Property
        /// <summary>
        /// Gets or sets the name of the method.
        /// </summary>
        /// <value>
        /// The name of the method.
        /// </value>
        public string MethodName { get; set; }
        #endregion
    }
    #endregion

    #region JSONPEncoderFactory
    /// <summary>
    /// 
    /// </summary>
    public class JSONPEncoderFactory : MessageEncoderFactory
    {
        #region .ctor
        JSONPEncoder encoder;
        /// <summary>
        /// Initializes a new instance of the <see cref="JSONPEncoderFactory" /> class.
        /// </summary>
        public JSONPEncoderFactory()
        {
            encoder = new JSONPEncoder();
        }
        #endregion

        #region Methods

        /// <summary>
        /// When overridden in a derived class, gets the message encoder that is produced by the factory.
        /// </summary>
        /// <returns>The <see cref="T:System.ServiceModel.Channels.MessageEncoder" /> used by the factory.</returns>
        public override MessageEncoder Encoder
        {
            get
            {
                return encoder;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets the message version that is used by the encoders produced by the factory to encode messages.
        /// </summary>
        /// <returns>The <see cref="T:System.ServiceModel.Channels.MessageVersion" /> used by the factory.</returns>
        public override MessageVersion MessageVersion
        {
            get { return encoder.MessageVersion; }
        }
        #endregion

        #region JSONPEncoder
        //JSONP encoder
        /// <summary>
        /// 
        /// </summary>
        class JSONPEncoder : MessageEncoder
        {
            private MessageEncoder encoder;

            public JSONPEncoder()
            {
                WebMessageEncodingBindingElement element = new WebMessageEncodingBindingElement();
                encoder = element.CreateMessageEncoderFactory().Encoder;
            }

            public override string ContentType
            {
                get
                {
                    return encoder.ContentType;
                }
            }

            public override string MediaType
            {
                get
                {
                    return encoder.MediaType;
                }
            }

            public override MessageVersion MessageVersion
            {
                get
                {
                    return encoder.MessageVersion;
                }
            }

            public override Message ReadMessage(ArraySegment<byte> buffer, BufferManager bufferManager, string contentType)
            {
                return encoder.ReadMessage(buffer, bufferManager, contentType);
            }

            public override Message ReadMessage(System.IO.Stream stream, int maxSizeOfHeaders, string contentType)
            {

                return encoder.ReadMessage(stream, maxSizeOfHeaders, contentType);
            }

            public override ArraySegment<byte> WriteMessage(Message message, int maxMessageSize, BufferManager bufferManager, int messageOffset)
            {
                MemoryStream stream = new MemoryStream();
                StreamWriter sw = new StreamWriter(stream);

                string methodName = null;
                if (message.Properties.ContainsKey(JSONPMessageProperty.Name))
                    methodName = ((JSONPMessageProperty)(message.Properties[JSONPMessageProperty.Name])).MethodName;

                if (methodName != null)
                {
                    sw.Write(methodName + "( ");
                    sw.Flush();
                }
                XmlWriter writer = JsonReaderWriterFactory.CreateJsonWriter(stream);
                message.WriteMessage(writer);
                writer.Flush();
                if (methodName != null)
                {
                    sw.Write(" );");
                    sw.Flush();
                }

                byte[] messageBytes = stream.GetBuffer();
                int messageLength = (int)stream.Position;
                int totalLength = messageLength + messageOffset;
                byte[] totalBytes = bufferManager.TakeBuffer(totalLength);
                Array.Copy(messageBytes, 0, totalBytes, messageOffset, messageLength);

                ArraySegment<byte> byteArray = new ArraySegment<byte>(totalBytes, messageOffset, messageLength);
                writer.Close();
                return byteArray;
            }

            public override void WriteMessage(Message message, System.IO.Stream stream)
            {
                string methodName = null;
                if (message.Properties.ContainsKey(JSONPMessageProperty.Name))
                    methodName = ((JSONPMessageProperty)(message.Properties[JSONPMessageProperty.Name])).MethodName;

                if (methodName == null)
                {
                    encoder.WriteMessage(message, stream);
                    return;
                }

                WriteToStream(stream, methodName + "( ");
                encoder.WriteMessage(message, stream);
                WriteToStream(stream, " );");
            }

            public void WriteToStream(Stream stream, string content)
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.Write(content);
                }
            }
            public override bool IsContentTypeSupported(string contentType)
            {
                return encoder.IsContentTypeSupported(contentType);
            }

        }
        #endregion
    } 
    #endregion

}
