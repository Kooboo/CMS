using System.Configuration;
using System.Web;
using System.Security.Permissions;

namespace Kooboo.Web.Mvc.WebResourceLoader.Configuration
{
    public class ReferenceElement : ConfigurationElement
    {
        private static ConfigurationProperty files;
        private static ConfigurationProperty nae;
        private static ConfigurationProperty mimeType;
        private static ConfigurationPropertyCollection properties;

        public ReferenceElement()
        {
            EnsureStaticPropertyBag();
        }

        private static ConfigurationPropertyCollection EnsureStaticPropertyBag()
        {
            if (properties == null)
            {
                files = new ConfigurationProperty("", typeof(FileCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
                nae = new ConfigurationProperty("name", typeof(string), string.Empty, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);
                mimeType = new ConfigurationProperty("mimeType", typeof(string), string.Empty, ConfigurationPropertyOptions.IsRequired);

                properties = new ConfigurationPropertyCollection
                                  {
                                      files,
                                      nae,
                                      mimeType
                                  };
            }
            return properties;
        }

        [ConfigurationProperty("name", DefaultValue = "")]
        public string Name
        {
            get { return (string)base[nae]; }
            set { base[nae] = value; }
        }

        [ConfigurationProperty("mimeType", DefaultValue = "text/text")]
        public string MimeType
        {
            get { return (string)base[mimeType]; }
            set { base[mimeType] = value; }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return EnsureStaticPropertyBag(); }
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public FileCollection Files
        {
            get { return (FileCollection)base[files]; }
        }
    }
}
