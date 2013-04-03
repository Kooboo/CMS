using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Persistence
{
    public interface IProviderFactory
    {
        string Name { get; }

        T GetProvider<T>();

        void RegisterProvider<ServiceType>(ServiceType provider);
    }
}
