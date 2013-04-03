using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Kooboo.Web.Mvc.WebResourceLoader.Configuration
{
    [ConfigurationCollection(typeof(ReferenceElement))]
    public sealed class ReferenceCollection : ConfigurationElementCollection
    {
        private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

        public ReferenceCollection() : base(StringComparer.OrdinalIgnoreCase) { }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ReferenceElement();
        }

        public ReferenceElement Get(int index)
        {
            return (ReferenceElement)base.BaseGet(index);
        }

        public ReferenceElement Get(string name)
        {
            return (ReferenceElement)base.BaseGet(name);
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ReferenceElement)element).Name;
        }

        public string GetKey(int index)
        {
            return (string)base.BaseGetKey(index);
        }

        // Properties
        public string[] AllKeys
        {
            get { return ObjectArrayToStringArray(base.BaseGetAllKeys()); }
        }

        public ReferenceElement this[int index]
        {
            get { return (ReferenceElement)base.BaseGet(index); }
        }

        public new ReferenceElement this[string name]
        {
            get { return (ReferenceElement)base.BaseGet(name); }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }

        protected override string ElementName
        {
            get { return "reference"; }
        }

        internal static string[] ObjectArrayToStringArray(object[] objectArray)
        {
            string[] array = new string[objectArray.Length];
            objectArray.CopyTo(array, 0);
            return array;
        }
    }
}
