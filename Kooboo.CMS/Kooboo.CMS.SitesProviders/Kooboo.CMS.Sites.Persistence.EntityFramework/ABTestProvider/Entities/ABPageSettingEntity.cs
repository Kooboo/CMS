using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.ABTest;
using Kooboo.CMS.Sites.Models;
using Kooboo.Common.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence.EntityFramework.ABTestProvider
{
    public interface IABPageSettingEntity
    {
        string UUID { get; set; }

        string SiteName { get; set; }

        string ObjectXml { get; set; }
    }

    public class ABPageSettingEntity : IABPageSettingEntity
    {
        public string UUID { get; set; }

        public string SiteName { get; set; }

        public string ObjectXml { get; set; }
    }

    public static class ABPageSettingExtensions
    {
        private static Type[] KnownTypes = new Type[]{
                typeof(ABPageSetting),
                typeof(IVisitRule),
                typeof(LanguageVisitRule),
                typeof(IPVisitRule),
                typeof(UserAgentVisitRule),
                typeof(QueryStringVisitRule),
                typeof(RandomVisitRule)
                };
        public static T ToABPageSettingEntity<T>(this ABPageSetting model)
            where T : IABPageSettingEntity, new()
        {
            return ToABPageSettingEntity(model, new T());
        }

        public static T ToABPageSettingEntity<T>(this ABPageSetting model, T entity)
            where T : IABPageSettingEntity
        {
            entity.UUID = model.UUID;
            if (null != model.Site)
            {
                entity.SiteName = model.Site.FullName;
            }

            entity.ObjectXml = DataContractSerializationHelper.SerializeAsXml(model, KnownTypes);
            return entity;
        }
        public static ABPageSetting ToABPageSetting(this IABPageSettingEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dummy = new ABPageSetting();
            dummy.UUID = entity.UUID;
            if (!String.IsNullOrEmpty(entity.SiteName))
            {
                dummy.Site = new Site(entity.SiteName);
            }

            var result = DataContractSerializationHelper.DeserializeFromXml<ABPageSetting>(entity.ObjectXml, KnownTypes);
            ((IPersistable)result).Init(dummy);

            return result;

        }
    }
}
