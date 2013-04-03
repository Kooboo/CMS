using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Kooboo.CMS.Sites.Persistence;

namespace Kooboo.CMS.Sites.Models
{
    public interface IPersistable
    {
        Site Site { get; set; }
        bool IsDummy { get; }
        void Init(IPersistable source);
        void OnSaved();
        void OnSaving();
        string DataFile { get; }
    }


    public static class IPersistableExtensions
    {
        public static T AsActual<T>(this T o)
            where T : IPersistable
        {
            if (o == null)
            {
                return default(T);
            }          
            if (o.IsDummy)
            {
                o = Persistence.Providers.ProviderFactory.GetRepository<IProvider<T>>().Get(o);
            }
            return o;
        }
    }
}
