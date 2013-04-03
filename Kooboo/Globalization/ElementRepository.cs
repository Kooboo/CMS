using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Globalization.Repository;

namespace Kooboo.Globalization
{
    public static class ElementRepository
    {
        public static IElementRepository DefaultRepository = new CacheElementRepository(new XmlElementRepository());
    }
}
