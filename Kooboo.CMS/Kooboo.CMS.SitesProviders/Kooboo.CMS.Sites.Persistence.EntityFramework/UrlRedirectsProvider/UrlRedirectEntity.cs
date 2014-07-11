using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using Kooboo.Common.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence.EntityFramework.UrlRedirectsProvider
{
    public interface IUrlRedirectEntity
    {
        string SiteName { get; set; }

        string UUID { get; set; }

        string ObjectXml { get; set; }
    }

    public class UrlRedirectEntity : IUrlRedirectEntity
    {
        public string SiteName { get; set; }
        public string UUID { get; set; }

        public string ObjectXml { get; set; }
    }

    public static class UrlRedirectExtensions
    {
        private static Type[] KnownTypes = new Type[]{
                typeof(UrlRedirect)
                };
        public static T ToUrlRedirectEntity<T>(this UrlRedirect model)
            where T : IUrlRedirectEntity, new()
        {
            return ToUrlRedirectEntity(model, new T());
        }
        public static T ToUrlRedirectEntity<T>(this UrlRedirect model, T entity)
            where T : IUrlRedirectEntity
        {
            entity.SiteName = model.Site.FullName;
            entity.UUID = model.UUID;
            entity.ObjectXml = DataContractSerializationHelper.SerializeAsXml(model, KnownTypes);
            return entity;
        }
        public static UrlRedirect ToUrlRedirect(this IUrlRedirectEntity entity)
        {
            if (entity == null)
            {
                return null;
            }
            UrlRedirect dummy;
            if (!String.IsNullOrWhiteSpace(entity.SiteName))
            {
                dummy = new UrlRedirect(new Site(entity.SiteName));
            }
            else
            {
                dummy = new UrlRedirect();
            }
            dummy.UUID = entity.UUID;
            var urlRedirect = DataContractSerializationHelper.DeserializeFromXml<UrlRedirect>(entity.ObjectXml, KnownTypes);
            ((IPersistable)urlRedirect).Init(dummy);
            return urlRedirect;
        }
    }
}
