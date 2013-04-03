using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Web.Mvc.WebResourceLoader.Configuration;

namespace Kooboo.Web.Mvc.WebResourceLoader
{
    public static class ConfigurationManager
    {
        static WebResourcesSection defaultSection;
        static IDictionary<string, WebResourcesSection> areasSection = new Dictionary<string, WebResourcesSection>(StringComparer.CurrentCultureIgnoreCase);

        static ConfigurationManager()
        {
            defaultSection = WebResourcesSection.GetSection();


        }
        public static void RegisterSection(string area, string configFile)
        {
            lock (areasSection)
            {
                WebResourcesSection section = WebResourcesSection.GetSection(configFile);
                areasSection.Add(area, section);
            }
        }
        public static WebResourcesSection GetSection(string area)
        {
            WebResourcesSection section = null;

            if (!string.IsNullOrEmpty(area) && areasSection.ContainsKey(area))
            {
                section = areasSection[area];
            }
            if (section == null)
            {
                section = defaultSection;
            }
            if (section == null)
            {
                throw new WebResourceException("Unable to find web resource configuraion setion.");
            }
            return section;
        }
    }
}
