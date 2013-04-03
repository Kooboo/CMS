using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Collections.Generic;

namespace NntpClientLib
{
    [Serializable]
    public class ArticleHeadersDictionary : Dictionary<string, List<string>>, IArticleHeadersProcessor, ISerializable
    {
        private string m_lastHeader;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleHeadersDictionary"/> class.
        /// </summary>
        public ArticleHeadersDictionary()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleHeadersDictionary"/> class.
        /// </summary>
        /// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> object containing the information required to serialize the <see cref="T:System.Collections.Generic.Dictionary`2"></see>.</param>
        /// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext"></see> structure containing the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.Dictionary`2"></see>.</param>
        protected ArticleHeadersDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            m_lastHeader = info.GetString("lastHeader");
        }

        /// <summary>
        /// Implements the <see cref="T:System.Runtime.Serialization.ISerializable"></see> interface and returns the data needed to serialize the <see cref="T:System.Collections.Generic.Dictionary`2"></see> instance.
        /// </summary>
        /// <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> object that contains the information required to serialize the <see cref="T:System.Collections.Generic.Dictionary`2"></see> instance.</param>
        /// <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext"></see> structure that contains the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.Dictionary`2"></see> instance.</param>
        /// <exception cref="T:System.ArgumentNullException">info is null.</exception>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            info.AddValue("lastHeader", m_lastHeader);
            base.GetObjectData(info, context);
        }

        #region IArticleHeadersProcessor Members

        public void AddHeader(string headerAndValue)
        {
            if (headerAndValue == null)
            {
                throw new ArgumentNullException("headerAndValue");
            }

            int idx = headerAndValue.IndexOf(": ");
            if (idx == -1)
            {
                if (string.IsNullOrEmpty(m_lastHeader))
                {
                    throw new NntpException();
                }
                if (headerAndValue.StartsWith(" ") || headerAndValue.StartsWith("\t"))
                {
                    AddHeader(m_lastHeader, headerAndValue.TrimStart());
                    return;
                }
            }
            int idxOfValue = idx + 2;
            string key = headerAndValue.Substring(0, idx);
            AddHeader(key, headerAndValue.Substring(idxOfValue));
        }

        public void AddHeader(string header, string value)
        {
            if (string.IsNullOrEmpty(header))
            {
                throw new ArgumentException("The header value must not be null or an empty string.", "header");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (!ContainsKey(header))
            {
                Add(header, new List<string>());
            }
            base[header].Add(value);
            m_lastHeader = header;
        }

        #endregion
    }
}

