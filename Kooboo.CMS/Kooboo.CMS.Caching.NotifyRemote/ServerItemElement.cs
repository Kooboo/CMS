using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;
using Kooboo.Collections;

namespace Kooboo.CMS.Caching.NotifyRemote
{
    public class ServerItemElement : ConfigurationElement
    {
        public ServerItemElement() { }
        public ServerItemElement(string elementName)
        {
            Name = elementName;
        }

        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

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
        public NameValueCollection UnrecognizedProperties
        {
            get
            {
                properties.MakeReadOnly();
                return properties;
            }
        }
        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            properties[name] = value;
            return true;
        }
    }
}
