using System.Configuration;
using System.Web;
using System.Security.Permissions;
using Kooboo.Configuration;

namespace Kooboo.Web.Mvc.WebResourceLoader.Configuration
{

    public sealed class WebResourcesSection : StandaloneConfigurationSection
    {
        private static ConfigurationProperty references;
        private static ConfigurationProperty version;
        private static ConfigurationProperty mode;
        private static ConfigurationProperty compact;
        private static ConfigurationProperty compress;
        private static ConfigurationProperty cacheDuration;

        private static ConfigurationPropertyCollection _properties;
        private static string SectionName = "webResources";

        public WebResourcesSection()
        {
            EnsureStaticPropertyBag();
        }

        private static ConfigurationPropertyCollection EnsureStaticPropertyBag()
        {
            if (_properties == null)
            {
                version = new ConfigurationProperty("version", typeof(string), "1.0", ConfigurationPropertyOptions.None);
                mode = new ConfigurationProperty("mode", typeof(Mode), Mode.Release, ConfigurationPropertyOptions.None);
                compact = new ConfigurationProperty("compact", typeof(bool), true, ConfigurationPropertyOptions.None);
                compress = new ConfigurationProperty("compress", typeof(bool), true, ConfigurationPropertyOptions.None);
                cacheDuration = new ConfigurationProperty("cacheDuration", typeof(int), 30, ConfigurationPropertyOptions.None);
                references = new ConfigurationProperty("references", typeof(ReferenceCollection), null, ConfigurationPropertyOptions.IsDefaultCollection | ConfigurationPropertyOptions.IsRequired);

                ConfigurationPropertyCollection propertys = new ConfigurationPropertyCollection
                                                                {
                                                                    references,
                                                                    version,
                                                                    mode,
                                                                    compact,
                                                                    compress,
                                                                    cacheDuration
                                                                };

                _properties = propertys;
            }
            return _properties;
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return EnsureStaticPropertyBag(); }
        }

        [ConfigurationProperty("version", DefaultValue = "1.0.0.0")]
        public string Version
        {
            get { return (string)base[version]; }
            set { base[version] = value; }
        }

        [ConfigurationProperty("mode", DefaultValue = Mode.Release)]
        public Mode Mode
        {
            get { return (Mode)base[mode]; }
            set { base[mode] = value; }
        }

        [ConfigurationProperty("compact", DefaultValue = true)]
        public bool Compact
        {
            get { return (bool)base[compact]; }
            set { base[compact] = value; }
        }

        [ConfigurationProperty("compress", DefaultValue = true)]
        public bool Compress
        {
            get { return (bool)base[compress]; }
            set { base[compress] = value; }
        }

        [ConfigurationProperty("cacheDuration", DefaultValue = 30)]
        public int CacheDuration
        {
            get { return (int)base[cacheDuration]; }
            set { base[cacheDuration] = value; }
        }

        [ConfigurationProperty("references", IsDefaultCollection = true)]
        public ReferenceCollection References
        {
            get { return (ReferenceCollection)base[references]; }
        }

        public static WebResourcesSection GetSection()
        {
            return (WebResourcesSection)System.Configuration.ConfigurationManager.GetSection(SectionName);
        }
        public static WebResourcesSection GetSection(string fileName)
        {
            WebResourcesSection section = new WebResourcesSection();
            section.GetSection(fileName, SectionName);
            return section;
        }
    }
}