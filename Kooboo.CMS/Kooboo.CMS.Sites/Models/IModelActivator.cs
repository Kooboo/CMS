using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Models
{
    public interface IModelActivator<T>
    {
        T Create(Site site, string name);
        T CreateDummy(Site site);
        T Create(string physicalPath);
    }
    public interface ICascadableModelActivator<T> : IModelActivator<T>
    {
        T Create(T parent, string name);
    }
}
