#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
#region Usings
using Kooboo.Collections;
using System.Collections.Specialized;
using System.Configuration;
#endregion

namespace Kooboo.CMS.Caching.NotifyRemote
{
    /// <summary>
    /// 
    /// </summary>
    public class ServerItemElement : ConfigurationElement
    {
        #region .ctor
        public ServerItemElement() { }
        public ServerItemElement(string elementName)
        {
            Name = elementName;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get { return (string)this["url"]; }
            set { this["url"] = value; }
        }

        /// <summary>
        /// Gets or sets the delay.
        /// </summary>
        /// <value>
        /// The delay.
        /// </value>
        [ConfigurationProperty("delay", IsRequired = false)]
        public int Delay
        {
            get { return (int)this["delay"]; }
            set { this["delay"] = value; }
        }

        private ReadonlyNameValueCollection properties = new ReadonlyNameValueCollection();
        /// <summary>
        /// Gets the unrecognized properties.
        /// </summary>
        /// <value>
        /// The unrecognized properties.
        /// </value>
        public NameValueCollection UnrecognizedProperties
        {
            get
            {
                properties.MakeReadOnly();
                return properties;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets a value indicating whether an unknown attribute is encountered during deserialization.
        /// </summary>
        /// <param name="name">The name of the unrecognized attribute.</param>
        /// <param name="value">The value of the unrecognized attribute.</param>
        /// <returns>
        /// true when an unknown attribute is encountered while deserializing; otherwise, false.
        /// </returns>
        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            properties[name] = value;
            return true;
        }
        #endregion
    }
}
