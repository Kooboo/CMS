using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Kooboo.Web.Mvc.Menu.Configuration
{
    public class MenuItemElementCollection : ConfigurationElementCollection
    {
        [ConfigurationProperty("type", IsRequired = false)]
        public string Type
        {
            get { return (string)this["type"]; }
            set { this["type"] = value; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new MenuItemElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MenuItemElement)element).Name;
        }

    }
}
