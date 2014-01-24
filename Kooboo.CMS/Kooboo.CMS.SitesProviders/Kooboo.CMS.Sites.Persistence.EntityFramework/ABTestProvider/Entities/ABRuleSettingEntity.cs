using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.ABTest;
using Kooboo.CMS.Sites.Models;
using Kooboo.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence.EntityFramework.ABTestProvider
{
    public interface IABRuleSettingEntity
    {
        int Id { get; set; }
        string UUID { get; set; }

        string SiteName { get; set; }

        string ObjectXml { get; set; }
    }

    public class ABRuleSettingEntity : IABRuleSettingEntity
    {
        public int Id { get; set; }

        public string UUID { get; set; }

        public string SiteName { get; set; }

        public string ObjectXml { get; set; }


    }

    public static class ABRuleSettingExtensions
    {
        private static Type[] KnownTypes = new Type[]{
                typeof(ABRuleSetting),
                typeof(IVisitRule),
                typeof(LanguageVisitRule),
                typeof(IPVisitRule),
                typeof(UserAgentVisitRule),
                typeof(QueryStringVisitRule),
                typeof(RandomVisitRule)
                };
        public static T ToABRuleSettingEntity<T>(this ABRuleSetting model)
            where T : IABRuleSettingEntity, new()
        {
            return ToABRuleSettingEntity(model, new T());
        }

        public static T ToABRuleSettingEntity<T>(this ABRuleSetting model, T entity)
            where T : IABRuleSettingEntity
        {
            entity.UUID = model.UUID;
            if (null != model.Site)
            {
                entity.SiteName = model.Site.FullName;
            }

            entity.ObjectXml = DataContractSerializationHelper.SerializeAsXml(model, KnownTypes);
            return entity;
        }
        public static ABRuleSetting ToABRuleSetting(this IABRuleSettingEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dummy = new ABRuleSetting();
            dummy.UUID = entity.UUID;
            if (!String.IsNullOrEmpty(entity.SiteName))
            {
                dummy.Site = new Site(entity.SiteName);
            }

            var result = DataContractSerializationHelper.DeserializeFromXml<ABRuleSetting>(entity.ObjectXml, KnownTypes);
            ((IPersistable)result).Init(dummy);

            return result;

        }
    }
}
