using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Globalization;
using Kooboo.CMS.Sites.Models;
using Kooboo.Globalization.Repository;

namespace Kooboo.CMS.Sites.Globalization
{
    public interface IRepositoryFactory
    {
        IElementRepository CreateRepository(Site site);
    }
    public class DefaultRepositoryFactory : IRepositoryFactory
    {
        public IElementRepository CreateRepository(Site site)
        {
            return new SiteLabelRepository(site);
        }
        private DefaultRepositoryFactory() { }

        static DefaultRepositoryFactory()
        {
            Instance = new DefaultRepositoryFactory();
        }
        public static IRepositoryFactory Instance
        {
            get;
            set;
        }
    }
}
