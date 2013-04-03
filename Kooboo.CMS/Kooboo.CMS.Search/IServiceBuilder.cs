using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Search
{
    public interface IServiceBuilder
    {
        ISearchService OpenService(Repository repository);
    }
    public class ServiceBuilder : IServiceBuilder
    {
        public ISearchService OpenService(Repository repository)
        {
            return new SearchService(repository);
        }
    }
}
