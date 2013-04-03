using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Kooboo.CMS.Caching.NotifyRemote
{
    public class ServerItemElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServerItemElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServerItemElement)element).Name;
        }

    }
}
