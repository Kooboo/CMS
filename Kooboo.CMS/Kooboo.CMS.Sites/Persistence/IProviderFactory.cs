using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace Kooboo.CMS.Sites.Persistence
{
    public interface IProviderFactory
    {
        T GetRepository<T>();

        void RegisterProvider<ServiceType>(ServiceType provider);
    }
}
