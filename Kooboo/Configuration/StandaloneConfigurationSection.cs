using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Xml.Linq;

namespace Kooboo.Configuration
{
    public class StandaloneConfigurationSection : ConfigurationSection
    {
        public virtual void GetSection(string fileName, string sectionName)
        {
            XDocument dom = XDocument.Load(fileName);
            base.DeserializeSection(dom.Elements().Where(e => e.Name == sectionName).First().CreateReader());
        }
    }
}
