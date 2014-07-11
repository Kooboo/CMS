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
    public interface IABPageTestResultEntity
    {
        string UUID { get; set; }

        string SiteName { get; set; }

        string ObjectXml { get; set; }
    }

    public class ABPageTestResultEntity : IABPageTestResultEntity
    {
        public string UUID { get; set; }

        public string SiteName { get; set; }

        public string ObjectXml { get; set; }
    }

    public static class ABPageTestResultExtensions
    {
        private static Type[] KnownTypes = new Type[]{
                typeof(ABPageTestResult),
                typeof(IVisitRule),
                typeof(LanguageVisitRule),
                typeof(IPVisitRule),
                typeof(UserAgentVisitRule),
                typeof(QueryStringVisitRule),
                typeof(RandomVisitRule)
                };
        public static T ToABPageTestResultEntity<T>(this ABPageTestResult model)
            where T : IABPageTestResultEntity, new()
        {
            return ToABPageTestResultEntity(model, new T());
        }

        public static T ToABPageTestResultEntity<T>(this ABPageTestResult model, T entity)
            where T : IABPageTestResultEntity
        {
            entity.UUID = model.UUID;
            if (null != model.Site)
            {
                entity.SiteName = model.Site.FullName;
            }

            entity.ObjectXml = DataContractSerializationHelper.SerializeAsXml(model, KnownTypes);
            return entity;
        }
        public static ABPageTestResult ToABPageTestResult(this IABPageTestResultEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dummy = new ABPageTestResult();
            dummy.UUID = entity.UUID;
            dummy.Site = new Site(entity.SiteName);

            var result = DataContractSerializationHelper.DeserializeFromXml<ABPageTestResult>(entity.ObjectXml, KnownTypes);
            ((IPersistable)result).Init(dummy);

            return result;

        }
    }
}
