using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using Kooboo.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence.EntityFramework.SiteProvider
{
    public interface ISiteSettingEntity
    {
        string ParentName { get; set; }

        string FullName { get; set; }

        string ObjectXml { get; set; }
    }

    public class SiteEntity : ISiteSettingEntity
    {
        public string FullName { get; set; }

        public string ParentName { get; set; }

        public string ObjectXml { get; set; }
    }

    public static class SiteEntityExtensions
    {
        private static Type[] KnownTypes = new Type[]{
                typeof(Site),
                typeof(PagePosition),
                typeof(ViewPosition),
                typeof(ModulePosition),
                typeof(HtmlPosition),
                typeof(ContentPosition),
                typeof(HtmlBlockPosition)
                };
        public static T ToSiteSettingEntity<T>(this Site model)
            where T : ISiteSettingEntity, new()
        {
            return ToSiteSettingEntity(model, new T());
        }
        public static T ToSiteSettingEntity<T>(this Site model, T entity)
            where T : ISiteSettingEntity
        {
            entity.FullName = model.FullName;
            if (null != model.Parent)
            {
                entity.ParentName = model.Parent.FullName;
            }
            entity.ObjectXml = DataContractSerializationHelper.SerializeAsXml(model, KnownTypes);
            return entity;
        }
        public static Site ToSite(this ISiteSettingEntity entity)
        {
            if (entity == null)
            {
                return null;
            }
            Site dummy;
            if (String.IsNullOrWhiteSpace(entity.ParentName))
            {
                dummy = new Site(entity.FullName);
            }
            else
            {
                dummy = new Site(new Site(entity.ParentName), entity.FullName);
            }
            var site = DataContractSerializationHelper.DeserializeFromXml<Site>(entity.ObjectXml, KnownTypes);
            ((IPersistable)site).Init(dummy);
            return site;
        }
    }
}
