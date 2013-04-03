using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Kooboo.Configuration;

namespace Kooboo.Web.Mvc.Menu.Configuration
{
    public class MenuSection : StandaloneConfigurationSection
    {
        static string SectionName = "menu";
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

        public static MenuSection GetSection()
        {
            return (MenuSection)ConfigurationManager.GetSection(SectionName);
        }

        public static MenuSection GetSection(string configFile)
        {
            MenuSection section = new MenuSection();
            section.GetSection(configFile, SectionName);
            return section;
        }
    }
}
