using System;
using System.Collections;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Kooboo.Web.Mvc.WebResourceLoader.Configuration
{
    [ConfigurationCollection(typeof(FileInfoElement)), AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public sealed class FileCollection : ConfigurationElementCollection
    {
        private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        public void Add(FileInfoElement namespaceInformation)
        {
            BaseAdd(namespaceInformation);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new FileInfoElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FileInfoElement)element).Filename;
        }

        public void Remove(string s)
        {
            BaseRemove(s);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public FileInfoElement this[int index]
        {
            get { return (FileInfoElement)base.BaseGet(index); }
            set
            {
                if (BaseGet(index) != null) {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }
    }
}
