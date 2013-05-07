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
using System.Configuration;
using System.Collections.Specialized;
using Kooboo.Collections;

namespace Kooboo.Web.Mvc.Menu.Configuration
{
    public class MenuItemElement : ConfigurationElement
    {
        public MenuItemElement() { }
        public MenuItemElement(string elementName)
        {
            Name = elementName;
        }

        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("text", IsRequired = false)]
        public string Text
        {
            get { return (string)this["text"]; }
            set { this["text"] = value; }
        }

        [ConfigurationProperty("action", IsRequired = false)]
        public string Action
        {
            get { return (string)this["action"]; }
            set { this["action"] = value; }
        }

        [ConfigurationProperty("controller", IsRequired = false)]
        public string Controller
        {
            get { return (string)this["controller"]; }
            set { this["controller"] = value; }
        }

        [ConfigurationProperty("area", IsRequired = false)]
        public string Area
        {
            get { return (string)this["area"]; }
            set { this["area"] = value; }
        }

        [ConfigurationProperty("visible", IsRequired = false, DefaultValue = true)]
        public bool Visible
        {
            get { return (bool)this["visible"]; }
            set { this["visible"] = value; }
        }
        [ConfigurationProperty("initializer", IsRequired = false)]
        public string Initializer
        {
            get { return (string)this["initializer"]; }
            set { this["initializer"] = value; }
        }

        [ConfigurationProperty("routeValues", IsRequired = false)]
        public RouteValuesElement RouteValues
        {
            get { return (RouteValuesElement)this["routeValues"]; }
            set { this["routeValues"] = value; }
        }

        [ConfigurationProperty("htmlAttributes", IsRequired = false)]
        public RouteValuesElement HtmlAttributes
        {
            get { return (RouteValuesElement)this["htmlAttributes"]; }
            set { this["htmlAttributes"] = value; }
        }

        [ConfigurationProperty("type", IsRequired = false)]
        public string Type
        {
            get { return (string)this["type"]; }
            set { this["type"] = value; }
        }

        [ConfigurationProperty("items", IsDefaultCollection = false)]
        public MenuItemElementCollection Items
        {
            get
            {
                MenuItemElementCollection itemsCollection =
                (MenuItemElementCollection)base["items"];
                return itemsCollection;
            }
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
