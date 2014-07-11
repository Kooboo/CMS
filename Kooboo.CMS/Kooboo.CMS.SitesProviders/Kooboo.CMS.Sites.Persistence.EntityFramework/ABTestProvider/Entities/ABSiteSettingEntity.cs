using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.ABTest;
using Kooboo.Common.Misc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence.EntityFramework.ABTestProvider
{
    public interface IABSiteSettingEntity
    {
        string UUID { get; set; }

        string SiteName { get; set; }

        string ObjectXml { get; set; }
    }

    public class ABSiteSettingEntity : IABSiteSettingEntity
    {
        public string UUID { get; set; }

        public string SiteName { get; set; }

        public string ObjectXml { get; set; }
    }

    public static class ABSiteSettingExtensions
    {
        private static Type[] KnownTypes = new Type[]{
                typeof(ABSiteSetting),
                typeof(IVisitRule),
                typeof(LanguageVisitRule),
                typeof(IPVisitRule),
                typeof(UserAgentVisitRule),
                typeof(QueryStringVisitRule),
                typeof(RandomVisitRule),
                typeof(ABSiteRuleItem),
                typeof(Kooboo.CMS.Sites.Models.RedirectType)
                };
        public static T ToABSiteSettingEntity<T>(this ABSiteSetting model)
            where T : IABSiteSettingEntity, new()
        {
            return ToABSiteSettingEntity(model, new T());
        }

        public static T ToABSiteSettingEntity<T>(this ABSiteSetting model, T entity)
            where T : IABSiteSettingEntity
        {
            entity.UUID = model.UUID;
            if (!String.IsNullOrEmpty(model.MainSite))
            {
                entity.SiteName = model.MainSite;
            }

            entity.ObjectXml = DataContractSerializationHelper.SerializeAsXml(model, KnownTypes);
            return entity;
        }
        public static ABSiteSetting ToABSiteSetting(this IABSiteSettingEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dummy = new ABSiteSetting();
            dummy.UUID = entity.UUID;
            dummy.MainSite = entity.SiteName;

            var result = DataContractSerializationHelper.DeserializeFromXml<ABSiteSetting>(entity.ObjectXml, KnownTypes);
            ((IPersistable)result).Init(dummy);

            return result;
        }
    }
}
