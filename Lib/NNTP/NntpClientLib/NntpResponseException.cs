using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace NntpClientLib
{
    [Serializable]
    public class NntpResponseException : NntpException, ISerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NntpResponseException"/> class.
        /// </summary>
        public NntpResponseException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NntpResponseException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public NntpResponseException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NntpResponseException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="lastResponse">The last response.</param>
        public NntpResponseException(string message, string lastResponse)
            : base(message)
        {
            m_lastResponse = lastResponse;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NntpResponseException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public NntpResponseException(string message, Exception inner)
            : base(message, inner)
        {
        }

        private string m_lastResponse;
        /// <summary>
        /// Gets the last response.
        /// </summary>
        /// <value>The last response.</value>
        public string LastResponse
        {
            get { return m_lastResponse; }
        }

        /// <summary>
        /// Gets the last response code.
        /// </summary>
        /// <value>The last response code.</value>
        public int LastResponseCode
        {
            get { return Convert.ToInt32(m_lastResponse.Substring(0, 3), System.Globalization.CultureInfo.InvariantCulture); }
        }

        #region ISerializable Members

        /// <summary>
        /// Initializes a new instance of the <see cref="NntpResponseException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
        /// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
        protected NntpResponseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            m_lastResponse = info.GetString("lastResponse");
        }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The info parameter is a null reference (Nothing in Visual Basic). </exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter"/></PermissionSet>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            info.AddValue("lastResponse", m_lastResponse);
            base.GetObjectData(info, context);
        }

        #endregion
    }
}

